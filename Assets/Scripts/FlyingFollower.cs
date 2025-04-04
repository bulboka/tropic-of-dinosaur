using UnityEngine;
using Random = UnityEngine.Random;

public class FlyingFollower : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _awakeDistance;
    [SerializeField] private float _sleepDistance;
    [SerializeField] private float _sleepIdleDurationMin;
    [SerializeField] private float _sleepIdleDurationMax;
    [SerializeField] private float _sleepMoveDurationMin;
    [SerializeField] private float _sleepMoveDurationMax;
    [SerializeField] private float _sleepMoveForce;
    [SerializeField] private float _awakeIdleDurationMin;
    [SerializeField] private float _awakeIdleDurationMax;
    [SerializeField] private float _awakeMoveDurationMin;
    [SerializeField] private float _awakeMoveDurationMax;
    [SerializeField] private float _awakeMoveForce;
    [SerializeField] private float _sleepRandomRadius;
    [SerializeField] private float _awakeDelay;
    [SerializeField] private bool _isFollowing = true;

    private bool _isAwake;
    private Vector3 _startPosition;
    private Vector3 _currentForce;
    private bool _isMoving;
    private float _nextChangeTime;
    private float _awakeDistanceSqr;
    private float _sleepDistanceSqr;

    private float GetNewIdleDuration() => _isAwake
        ? Random.Range(_awakeIdleDurationMin, _awakeIdleDurationMax)
        : Random.Range(_sleepIdleDurationMin, _sleepIdleDurationMax);

    private float GetNewMoveDuration() => _isAwake
        ? Random.Range(_awakeMoveDurationMin, _awakeMoveDurationMax)
        : Random.Range(_sleepMoveDurationMin, _sleepMoveDurationMax);

    private void Start()
    {
        _startPosition = transform.position;
        _sleepDistanceSqr = Mathf.Pow(_sleepDistance, 2);
        _awakeDistanceSqr = Mathf.Pow(_awakeDistance, 2);
    }

    private void Update()
    {
        if (Time.time < _nextChangeTime)
        {
            return;
        }

        var distToPlayerSqr = (GameSession.Body.Torso.position - transform.position).sqrMagnitude;

        if (_isAwake && distToPlayerSqr > _sleepDistanceSqr)
        {
            _isAwake = false;
        }

        if (!_isAwake && distToPlayerSqr < _awakeDistanceSqr)
        {
            _isAwake = true;
        }

        _isMoving = !_isMoving;
        _nextChangeTime = Time.time + (_isMoving ? GetNewMoveDuration() : GetNewIdleDuration());

        if (!_isMoving)
        {
            return;
        }

        var newTarget = _isAwake
            ? _isFollowing
                ? GameSession.Body.Torso.position
                : transform.position + (transform.position - GameSession.Body.Torso.position)
            : new Vector3(_startPosition.x + Random.Range(-_sleepRandomRadius, _sleepRandomRadius),
                _startPosition.y + Random.Range(-_sleepRandomRadius, _sleepRandomRadius), _startPosition.z);

        _currentForce = (newTarget - transform.position).normalized * _sleepMoveForce;
    }

    private void FixedUpdate()
    {
        if (_isMoving && _currentForce != Vector3.zero)
        {
            _rigidbody.AddForce(_currentForce);
        }
    }
}