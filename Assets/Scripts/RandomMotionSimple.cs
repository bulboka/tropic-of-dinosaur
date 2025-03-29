using UnityEngine;
using Random = UnityEngine.Random;

public class RandomMotionSimple : MonoBehaviour
{
    [SerializeField] private bool _randomizePosition = true;
    [SerializeField] private float _maxShiftX;
    [SerializeField] private float _maxShiftY;
    [SerializeField] private bool _randomizeRotation = true;
    [SerializeField] private float _minAngle;
    [SerializeField] private float _maxAngle;
    [SerializeField] private float _minPerionDuration;
    [SerializeField] private float _maxPerionDuration;

    private Vector3 _initialPosition;
    private float _initialAngle;
    private float _nextChangeTime;

    private void Start()
    {
        _initialPosition = transform.localPosition;
        _initialAngle = transform.localRotation.eulerAngles.z;
    }

    private void Update()
    {
        if (Time.time < _nextChangeTime)
        {
            return;
        }

        _nextChangeTime = Time.time + Random.Range(_minPerionDuration, _maxPerionDuration);

        if (_randomizePosition)
        {
            transform.localPosition = new Vector3(_initialPosition.x + Random.Range(-_maxShiftX, _maxShiftX),
                _initialPosition.y + Random.Range(-_maxShiftY, _maxShiftY), _initialPosition.z);
        }

        if (_randomizeRotation)
        {
            transform.localRotation = Quaternion.Euler(0, 0, _initialAngle + Random.Range(_minAngle, _maxAngle));
        }
    }
}