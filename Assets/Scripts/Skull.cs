using System;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public enum SkullState { ApproachPrey, Recharge, FlyAway, Follow }

public class Skull : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Transform _view;
    [SerializeField] private float _startPostionDistance;
    [SerializeField] private float _approachForce;
    [SerializeField] private float _rechargeForce;
    [SerializeField] private float _minApproachDistance;
    [SerializeField] private float _rechargeDistance;
    [SerializeField] private float _minRechargeDistance;
    [SerializeField] private float _flyAwayDistance;
    [SerializeField] private float _followForce;
    [SerializeField] private float _followRandomRadius;
    [SerializeField] private float _minFollowTargetDistance;

    public Action<Skull, Transform> OnPreyEaten;

    private SkullState _state;
    private Vector3 _targetPosition;
    private Transform _currentPrey;
    private Vector3 _flyAwayForce;

    public void Initialize()
    {
        var toStartPositionVector = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360f))) *
                                    new Vector3(_startPostionDistance, 0, 0);
        var startPosition = GameSession.Body.Torso.transform.position + toStartPositionVector;

        _rigidbody.MovePosition(startPosition);
        transform.position = startPosition;
        Prey();
    }

    private void Prey()
    {
        _currentPrey = GameSession.SkullManager.GetNextPrey();

        if (_currentPrey == null)
        {
            FlyAway();
        }
        else
        {
            _state = SkullState.ApproachPrey;
        }
    }

    private void Update()
    {
        _view.localScale = new Vector3(_rigidbody.velocity.x > 0 ? 1f : -1f, 1f, 1f);

    }

    private void FixedUpdate()
    {
        switch (_state)
        {
            case SkullState.ApproachPrey:
                FixedUpdateApproachPrey();
                break;

            case SkullState.Recharge:
                FixedUpdateRecharge();
                break;

            case SkullState.FlyAway:
                FixedUpdateFlyAway();
                break;

            case SkullState.Follow:
                FixedUpdateFollow();
                break;
        }
    }

    private void FixedUpdateFollow()
    {
        if (_state != SkullState.Follow)
        {
            return;
        }

        var toTargetVector = _targetPosition - transform.position;

        _rigidbody.AddForce(toTargetVector.normalized * _rechargeForce);

        if (toTargetVector.sqrMagnitude <= _minFollowTargetDistance * _minFollowTargetDistance)
        {
            PickNextFollowTargetPosition();
        }
    }

    private void PickNextFollowTargetPosition()
    {
        _targetPosition = GameSession.Body.Torso.transform.position;
        _targetPosition.x += Random.Range(-_followRandomRadius, _followRandomRadius);
        _targetPosition.y += Random.Range(-_followRandomRadius, _followRandomRadius);
    }

    private void FixedUpdateFlyAway()
    {
        if (_state != SkullState.FlyAway)
        {
            return;
        }

        _rigidbody.AddForce(_flyAwayForce);

        if ((transform.position - GameSession.Body.Torso.transform.position).sqrMagnitude >=
            _flyAwayDistance * _flyAwayDistance)
        {
            OnPreyEaten = null;
            Destroy(gameObject);
        }
    }

    private void FixedUpdateRecharge()
    {
        if (_state != SkullState.Recharge)
        {
            return;
        }

        var toTargetVector = _targetPosition - transform.position;

        _rigidbody.AddForce(toTargetVector.normalized * _rechargeForce);

        if (toTargetVector.sqrMagnitude <= _minRechargeDistance * _minRechargeDistance)
        {
            Prey();
        }
    }

    private void FixedUpdateApproachPrey()
    {
        if (_state != SkullState.ApproachPrey)
        {
            return;
        }

        var toTargetVector = _currentPrey.position - transform.position;

        _rigidbody.AddForce(toTargetVector.normalized * _approachForce);

        if (toTargetVector.sqrMagnitude <= _minApproachDistance * _minApproachDistance)
        {
            _targetPosition = transform.position + (Vector3)_rigidbody.velocity.normalized * _rechargeDistance;
            _state = SkullState.Recharge;

            OnPreyEaten?.Invoke(this, _currentPrey);
            Destroy(_currentPrey.gameObject);
            _currentPrey = null;
        }
    }

    public void FlyAway()
    {
        _state = SkullState.FlyAway;
        _flyAwayForce = _rigidbody.velocity.normalized * _rechargeForce;
    }

    public void Follow()
    {
        _state = SkullState.Follow;
        PickNextFollowTargetPosition();
    }
}