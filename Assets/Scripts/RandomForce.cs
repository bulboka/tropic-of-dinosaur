using UnityEngine;
using Random = UnityEngine.Random;

public class RandomForce : MonoBehaviour
{
    [SerializeField] private Vector2 _forceMin;
    [SerializeField] private Vector2 _forceMax;
    [SerializeField] private float _periodDurationMin;
    [SerializeField] private float _periodDurationMax;
    [SerializeField] private Rigidbody2D _rigidbody;

    private float _nextPeriodTime;

    private void Start()
    {
        if (_rigidbody == null)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }
    }

    private void Update()
    {
        if (Time.time < _nextPeriodTime)
        {
            return;
        }

        _nextPeriodTime = Time.time + Random.Range(_periodDurationMin, _periodDurationMax);
        _rigidbody.AddForce(new Vector2(Random.Range(_forceMin.x, _forceMax.x), Random.Range(_forceMin.y, _forceMax.y)));
    }
}