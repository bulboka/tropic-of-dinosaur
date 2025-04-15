using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Body : MonoBehaviour
{
    [SerializeField] private Hand _hand;
    [SerializeField] private Rigidbody2D _torso;
    [SerializeField] private Transform _skeletonRoot;
    [SerializeField] private Transform _rootBone;

    public Hand Hand => _hand;

    public Rigidbody2D Torso => _torso;

    public Transform SkeletonRoot => _skeletonRoot;

    public bool IsTouchingGround => _isTouchingGround;

    public Transform RootBone => _rootBone;

    private List<Rigidbody2D> _limbs;
    private bool _isInitialized;
    private ContactPoint2D[] _contactPoints;
    private ContactFilter2D _contactFilter;
    private Collider2D[] _overlapColliders;
    private bool _isTouchingGround;

    public void Initialize()
    {
        if (_hand != null)
        {
            _hand.transform.SetParent(null);
            _hand.Initialize(this);
        }

        _contactPoints = new ContactPoint2D[20];
        _overlapColliders = new Collider2D[20];
        _contactFilter = new ContactFilter2D();
        _contactFilter.SetLayerMask(LayerMask.GetMask("Ground", "TrapDoor", "ChickenHeartsGround", "Tangerine"));
        _contactFilter.useTriggers = true;
        _limbs = GetComponentsInChildren<Rigidbody2D>().ToList();
        _isInitialized = true;
    }

    private void Update()
    {
        if (!_isInitialized)
        {
            return;
        }

        _isTouchingGround = _limbs.Any(limb =>
            limb.GetContacts(_contactFilter, _contactPoints) > 0 ||
            limb.OverlapCollider(_contactFilter, _overlapColliders) > 0);
    }
}