using UnityEngine;
using Random = UnityEngine.Random;

public class RandomTorque : MonoBehaviour
{
    [SerializeField] private float _torqueMin;
    [SerializeField] private float _torqueMax;
    [SerializeField] private float _periodDurationMin;
    [SerializeField] private float _periodDurationMax;
    [SerializeField] private Rigidbody2D _rigidbody;

    private float _nextPeriodTime;
    private float _mult = 1f;

    private void Update()
    {
        if (Time.time < _nextPeriodTime)
        {
            return;
        }

        _nextPeriodTime = Time.time + Random.Range(_periodDurationMin, _periodDurationMax);
        _rigidbody.AddTorque(Random.Range(_torqueMin, _torqueMax) * _mult);
        _mult *= -1f;
    }
}