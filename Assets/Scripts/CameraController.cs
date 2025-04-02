using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _smoothTime;
    [SerializeField] private float _margin;
    [SerializeField] private Transform _overlayEffectContainer;
    [SerializeField] private Camera _camera;

    private Vector3 _velocity;
    private Vector3 _innerVelocity;
    private float _maxX;
    private GameObject _overlayEffect;
    private Vector3 _shift;
    private List<CameraShiftTrigger> _shiftTriggers;
    private float _zoom;

    public Camera Camera => _camera;

    private void Start()
    {
        _maxX = transform.position.x;

        if (_overlayEffectContainer.childCount > 0)
        {
            _overlayEffect = _overlayEffectContainer.GetChild(0).gameObject;
        }

        _shiftTriggers = new List<CameraShiftTrigger>();
        _zoom = _camera.transform.localPosition.z;
    }

    private void LateUpdate()
    {
        if (_target == null)
        {
            return;
        }

        var targetPosition = _target.position + _shift;
        targetPosition.x = Mathf.Max(targetPosition.x, _maxX - _margin);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _smoothTime);
        _maxX = Mathf.Max(_maxX, transform.position.x);

        _camera.transform.localPosition = Vector3.SmoothDamp(_camera.transform.localPosition, new Vector3(0, 0, _zoom),
            ref _innerVelocity, _smoothTime);
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void SwitchOverlayEffect(GameObject effectPrefab)
    {
        if (_overlayEffect != null)
        {
            Destroy(_overlayEffect.gameObject);
        }

        _overlayEffect = Instantiate(effectPrefab, _overlayEffectContainer);
        _overlayEffect.transform.localPosition = Vector3.zero;
        _overlayEffect.transform.localRotation = Quaternion.identity;
        _overlayEffect.transform.localScale = Vector3.one;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var shiftTrigger = other.gameObject.GetComponent<CameraShiftTrigger>();

        if (shiftTrigger != null)
        {
            _shiftTriggers.Add(shiftTrigger);
            _shift = shiftTrigger.Shift;

            return;
        }

        var zoomTrigger = other.gameObject.GetComponent<CameraZoomTrigger>();

        if (zoomTrigger != null)
        {
            _zoom = zoomTrigger.Zoom;
            zoomTrigger.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var shiftTrigger = other.gameObject.GetComponent<CameraShiftTrigger>();

        if (shiftTrigger == null)
        {
            return;
        }

        if (_shiftTriggers.Remove(shiftTrigger))
        {
            _shift = _shiftTriggers.Count == 0 ? Vector3.zero : _shiftTriggers.Last().Shift;
        }
    }
}
