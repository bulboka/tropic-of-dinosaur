using UnityEngine;

public class RollingFollower : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _minDistance;
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _torque;
    [SerializeField] private float _awakeDistance;
    [SerializeField] private float _sleepDistance;

    private float _minDistSqr;
    private float _maxDistSqr;
    private float _awakeDistanceSqr;
    private float _sleepDistanceSqr;
    private bool _isAwake;

    private void Start()
    {
        _minDistSqr = Mathf.Pow(_minDistance, 2);
        _maxDistSqr = Mathf.Pow(_maxDistance, 2);
        _awakeDistanceSqr = Mathf.Pow(_awakeDistance, 2);
        _sleepDistanceSqr = Mathf.Pow(_sleepDistance, 2);
    }

    private void Update()
    {
        var distToPlayerSqr = (GameSession.Body.Torso.position - transform.position).sqrMagnitude;

        if (!_isAwake && distToPlayerSqr < _awakeDistanceSqr)
        {
            _isAwake = true;
        }

        if (!_isAwake)
        {
            return;
        }

        if (distToPlayerSqr > _sleepDistanceSqr)
        {
            _isAwake = false;
            return;
        }

        if (distToPlayerSqr < _minDistSqr || distToPlayerSqr > _maxDistSqr)
        {
            return;
        }

        _rigidbody.AddTorque(GameSession.Body.Torso.position.x > transform.position.x ? _torque : -_torque);
    }
}
