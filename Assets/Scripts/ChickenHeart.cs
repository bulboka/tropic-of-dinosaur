using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ChickenHeartState { Sleeping, Awake, FreeFollowing }

public class ChickenHeart : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _awakeIdleDurationMin;
    [SerializeField] private float _awakeIdleDurationMax;
    [SerializeField] private float _awakeIdleDurationFastMin;
    [SerializeField] private float _awakeIdleDurationFastMax;
    [SerializeField] private float _freeFollowIdleDurationMin;
    [SerializeField] private float _freeFollowIdleDurationMax;
    [SerializeField] private float _awakeMoveDurationMin;
    [SerializeField] private float _awakeMoveDurationMax;
    [SerializeField] private float _freeFollowMoveDurationMin;
    [SerializeField] private float _freeFollowMoveDurationMax;
    [SerializeField] private float _moveForce;
    [SerializeField] private float _moveFastForce;
    [SerializeField] private float _moveFreeFollowForce;
    [SerializeField] private float _randomRadius;
    [SerializeField] private float _randomRadiusFreeFollow;
    [SerializeField] private float _freeFollowLiftingForce;
    [SerializeField] private float _wheelTorque;

    [Header("Special")]
    [SerializeField] private float _specialMoveForce;
    [SerializeField] private float _specialMoveFastForce;
    [SerializeField] private float _specialAwakeIdleDurationFastMin;
    [SerializeField] private float _specialAwakeIdleDurationFastMax;

    private ChickenHeartState _state;
    private Vector3 _startPosition;
    private Vector3 _currentForce;
    private bool _isMoving;
    private float _nextChangeTime;
    private float _awakeDistanceSqr;
    private float _sleepDistanceSqr;
    private bool _isFarFromStartPosition;
    private float _randomRadiusSqr;

    public Action<ChickenHeart> OnStateChange;

    public ChickenHeartState State
    {
        get => _state;
        set
        {
            _state = value;
            OnStateChange?.Invoke(this);
        }
    }

    public float FreeFollowLiftingForce
    {
        get => _freeFollowLiftingForce;
        set => _freeFollowLiftingForce = value;
    }

    private float GetNewIdleDuration() => _state == ChickenHeartState.FreeFollowing
        ? Random.Range(_freeFollowIdleDurationMin, _freeFollowIdleDurationMax)
        : _isFarFromStartPosition
            ? Random.Range(_awakeIdleDurationFastMin, _awakeIdleDurationFastMax)
            : Random.Range(_awakeIdleDurationMin, _awakeIdleDurationMax);

    private float GetNewMoveDuration() => _state == ChickenHeartState.FreeFollowing
        ? Random.Range(_freeFollowMoveDurationMin, _freeFollowMoveDurationMax)
        : Random.Range(_awakeMoveDurationMin, _awakeMoveDurationMax);

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

        if (_state == ChickenHeartState.FreeFollowing)
        {
            _startPosition = GameSession.Body.Torso.position;
        }

        _isFarFromStartPosition = (transform.position - _startPosition).sqrMagnitude > _randomRadiusSqr;
        _isMoving = !_isMoving;
        _nextChangeTime = Time.time + (_isMoving ? GetNewMoveDuration() : GetNewIdleDuration());

        if (!_isMoving)
        {
            return;
        }

        var randomRadius = _state == ChickenHeartState.FreeFollowing ? _randomRadiusFreeFollow : _randomRadius;
        var newTarget =  new Vector3(_startPosition.x + Random.Range(-randomRadius, randomRadius),
                _startPosition.y + Random.Range(-randomRadius, randomRadius), _startPosition.z);

        _currentForce = (newTarget - transform.position).normalized * (_state == ChickenHeartState.FreeFollowing
            ?
            _moveFreeFollowForce
            : _isFarFromStartPosition
                ? _moveFastForce
                : _moveForce);
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

    private void OnCollisionStay2D(Collision2D other)
    {
        if (_state != ChickenHeartState.FreeFollowing || !other.gameObject.CompareTag("Body"))
        {
            return;
        }

        other.rigidbody.AddForce(new Vector2(0, FreeFollowLiftingForce * Time.deltaTime));
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("ChickenWheel"))
        {
            return;
        }

        other.attachedRigidbody.AddTorque(_wheelTorque * Time.deltaTime * (other.transform.localScale.x > 0 ? 1f : -1f));
    }

    public void BecomeSpecial()
    {
        _moveForce = _specialMoveForce;
        _moveFastForce = _specialMoveFastForce;
        _awakeIdleDurationFastMin = _specialAwakeIdleDurationFastMin;
        _awakeIdleDurationFastMax = _specialAwakeIdleDurationFastMax;
    }
}
