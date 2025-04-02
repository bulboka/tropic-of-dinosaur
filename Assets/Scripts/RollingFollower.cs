using UnityEngine;

public class RollingFollower : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _minDistance;
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _torque;
    [SerializeField] private float _awakeDistance;
    [SerializeField] private float _sleepDistance;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _activationDistance;

    private float _minDistSqr;
    private float _maxDistSqr;
    private float _awakeDistanceSqr;
    private float _sleepDistanceSqr;
    private float _activationDistanceSqr;
    private bool _isAwake;
    private bool _isActivated;

    private bool IsAwake
    {
        get => _isAwake;
        set
        {
            _isAwake = value;

            if (!_isAwake)
            {
                _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                _spriteRenderer.sortingOrder = -1;
            }
        }
    }

    private void Start()
    {
        _minDistSqr = Mathf.Pow(_minDistance, 2);
        _maxDistSqr = Mathf.Pow(_maxDistance, 2);
        _awakeDistanceSqr = Mathf.Pow(_awakeDistance, 2);
        _sleepDistanceSqr = Mathf.Pow(_sleepDistance, 2);
        _activationDistanceSqr = Mathf.Pow(_activationDistance, 2);
    }

    private void Update()
    {
        var distToPlayerSqr = (GameSession.Body.Torso.position - transform.position).sqrMagnitude;

        if (!IsAwake && distToPlayerSqr < _awakeDistanceSqr)
        {
            IsAwake = true;
        }

        if (!IsAwake)
        {
            return;
        }

        if (distToPlayerSqr > _sleepDistanceSqr)
        {
            IsAwake = false;
            return;
        }

        if (distToPlayerSqr < _minDistSqr || distToPlayerSqr > _maxDistSqr)
        {
            return;
        }

        if (!_isActivated && distToPlayerSqr > _activationDistanceSqr)
        {
            _isActivated = true;
            _rigidbody.constraints = RigidbodyConstraints2D.None;
            _spriteRenderer.sortingOrder = 1;
        }

        _rigidbody.AddTorque(GameSession.Body.Torso.position.x > transform.position.x ? -_torque : _torque);
    }
}
