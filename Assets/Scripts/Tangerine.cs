using System;
using UnityEngine;

public class Tangerine : MonoBehaviour
{
    [SerializeField] private Transform _startLocator;
    [SerializeField] private Transform _finishLocator;
    [SerializeField] private float _scaleMin;
    [SerializeField] private float _scaleMax;
    [SerializeField] private float _cameraZoomStart;
    [SerializeField] private float _cameraZoomFinish;
    [SerializeField] private Vector3 _cameraShiftStart;
    [SerializeField] private Vector3 _cameraShiftFinish;
    [SerializeField] private AnimationCurve _factorCurve;

    private void Start()
    {
        GameSession.Hand.MaxDistanceFromCamera = 20f;
    }

    private void Update()
    {
        var progress = Mathf.Clamp01((transform.position.x - _startLocator.position.x) /
                                   (_finishLocator.position.x - _startLocator.position.x));
        var factor = _factorCurve.Evaluate(progress);

        transform.localScale = Vector3.one * (_scaleMin + (_scaleMax - _scaleMin) * factor);

        GameSession.Camera.Zoom = Mathf.Lerp(_cameraZoomStart, _cameraZoomFinish, factor);
        GameSession.Camera.Shift = Vector3.Lerp(_cameraShiftStart, _cameraShiftFinish, factor);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("TangerineDeath"))
        {
            Destroy(gameObject);
        }
    }
}