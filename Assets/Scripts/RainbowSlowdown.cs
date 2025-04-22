using UnityEngine;

public class RainbowSlowdown : MonoBehaviour
{
    [SerializeField] private Transform _startLocator;
    [SerializeField] private Transform _finishLocator;
    [SerializeField] private AreaEffector2D _effector;
    [SerializeField] private float _startForce;
    [SerializeField] private float _finishForce;
    [SerializeField] private AnimationCurve _curve;

    private void Update()
    {
        var progress = Mathf.Clamp01((GameSession.Body.Torso.transform.position.y - _startLocator.position.y) /
                                     (_finishLocator.position.y - _startLocator.position.y));
        var factor = _curve.Evaluate(progress);

        _effector.forceMagnitude = _startForce + (_finishForce - _startForce) * factor;
    }
}