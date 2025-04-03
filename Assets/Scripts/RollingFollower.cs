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
    [SerializeField] private float _dragOnGround;
    [SerializeField] private float _dragNotOnGround;
    [SerializeField] private float _notOnGroundForce;
    [SerializeField] private float _notOnGroundTorque;

    private float _minDistSqr;
    private float _maxDistSqr;
    private float _awakeDistanceSqr;
    private float _sleepDistanceSqr;
    private float _activationDistanceSqr;
    private bool _isAwake;
    private bool _isActivated;
    private ContactPoint2D[] _contactPoints;
    private ContactFilter2D _contactFilter;

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

        _contactPoints = new ContactPoint2D[20];
        _contactFilter = new ContactFilter2D();
        _contactFilter.SetLayerMask(LayerMask.GetMask("Ground"));
    }

    private void Update()
    {
        var toPlayerVector = GameSession.Body.Torso.position - transform.position;
        var distToPlayerSqr = toPlayerVector.sqrMagnitude;

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

        var isOnGround = _rigidbody.GetContacts(_contactFilter, _contactPoints) > 0;
        _rigidbody.drag = isOnGround ? _dragOnGround : _dragNotOnGround;

        var torque = (GameSession.Body.Torso.position.x > transform.position.x ? -1 : 1) *
                     (isOnGround ? _torque : _notOnGroundTorque) * Time.deltaTime;
        _rigidbody.AddTorque(torque);

        if (!isOnGround)
        {
            _rigidbody.AddForce(toPlayerVector.normalized * (_notOnGroundForce * Time.deltaTime));
        }
    }
}
