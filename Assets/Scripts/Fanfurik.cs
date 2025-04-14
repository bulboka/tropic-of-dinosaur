using UnityEngine;
using Random = UnityEngine.Random;

public class Fanfurik : MonoBehaviour
{
    [SerializeField] private float _periodMin;
    [SerializeField] private float _periodMax;
    [SerializeField] private float _emissionDuration;
    [SerializeField] private AnimationCurve _emissionForceCurve;
    [SerializeField] private AreaEffector2D _effector;
    [SerializeField] private ParticleSystem _particles;
    [SerializeField] private float _effectorForceBase;
    [SerializeField] private Animator _animator;

    private bool _isEmitting;
    private float _nextEmissionTime;
    private float _currentEmissionStartTime;
    private static readonly int StartEmittingCached = Animator.StringToHash("StartEmitting");

    private void Start()
    {
        _nextEmissionTime = Time.time + Random.Range(_periodMin, _periodMax);
        _effector.gameObject.SetActive(false);
    }

    private void Update()
    {
        TryStartEmission();
        UpdateEmission();
    }

    private void UpdateEmission()
    {
        if (!_isEmitting)
        {
            return;
        }

        if (Time.time >= _currentEmissionStartTime + _emissionDuration)
        {
            _isEmitting = false;
            _effector.gameObject.SetActive(false);
        }
        else
        {
            _effector.forceMagnitude = _effectorForceBase *
                                       _emissionForceCurve.Evaluate((Time.time - _currentEmissionStartTime) /
                                                                    _emissionDuration);
        }
    }

    private void TryStartEmission()
    {
        if (_isEmitting || Time.time < _nextEmissionTime)
        {
            return;
        }

        _isEmitting = true;
        _currentEmissionStartTime = Time.time;
        _nextEmissionTime = Time.time + _emissionDuration + Random.Range(_periodMin, _periodMax);
        _effector.gameObject.SetActive(true);
        _particles.Play();
        _animator.SetTrigger(StartEmittingCached);
    }
}