using UnityEngine;
using Random = UnityEngine.Random;

public enum ChickenHeartState { Sleeping, Awake }

public class ChickenHeart : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _awakeIdleDurationMin;
    [SerializeField] private float _awakeIdleDurationMax;
    [SerializeField] private float _awakeIdleDurationFastMin;
    [SerializeField] private float _awakeIdleDurationFastMax;
    [SerializeField] private float _awakeMoveDurationMin;
    [SerializeField] private float _awakeMoveDurationMax;
    [SerializeField] private float _moveForce;
    [SerializeField] private float _moveFastForce;
    [SerializeField] private float _randomRadius;

    private ChickenHeartState _state;

    private Vector3 _startPosition;
    private Vector3 _currentForce;
    private bool _isMoving;
    private float _nextChangeTime;
    private float _awakeDistanceSqr;
    private float _sleepDistanceSqr;
    private bool _isFarFromStartPosition;
    private float _randomRadiusSqr;

    public ChickenHeartState State
    {
        get => _state;
        set => _state = value;
    }

    private float GetNewIdleDuration() => _isFarFromStartPosition
        ? Random.Range(_awakeIdleDurationFastMin, _awakeIdleDurationFastMax)
        : Random.Range(_awakeIdleDurationMin, _awakeIdleDurationMax);

    private float GetNewMoveDuration() => Random.Range(_awakeMoveDurationMin, _awakeMoveDurationMax);

    private void Start()
    {
        _startPosition = transform.position;
        _randomRadiusSqr = Mathf.Pow(_randomRadius * 1.4f, 2);
    }

    private void Update()
    {
        if (Time.time < _nextChangeTime)
        {
            return;
        }

        _isFarFromStartPosition = (transform.position - _startPosition).sqrMagnitude > _randomRadiusSqr;
        _isMoving = !_isMoving;
        _nextChangeTime = Time.time + (_isMoving ? GetNewMoveDuration() : GetNewIdleDuration());

        if (!_isMoving)
        {
            return;
        }

        var newTarget =  new Vector3(_startPosition.x + Random.Range(-_randomRadius, _randomRadius),
                _startPosition.y + Random.Range(-_randomRadius, _randomRadius), _startPosition.z);

        _currentForce = (newTarget - transform.position).normalized * (_isFarFromStartPosition ? _moveFastForce : _moveForce);
    }

    private void FixedUpdate()
    {
        if (_isMoving && _currentForce != Vector3.zero)
        {
            _rigidbody.AddForce(_currentForce);
        }
    }

    public void GoToTrigger(ChickenHeartsTrigger chickenHeartsTrigger)
    {
        if (State != ChickenHeartState.Awake)
        {
            return;
        }

        _startPosition = chickenHeartsTrigger.GetNextChickenHeartPosition();
    }
}
