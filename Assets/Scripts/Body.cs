using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Body : MonoBehaviour
{
    [SerializeField] private Hand _hand;
    [SerializeField] private Rigidbody2D _torso;
    [SerializeField] private Transform _skeletonRoot;
    [SerializeField] private Transform _rootBone;
    [SerializeField] private float _transitionDuration;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform _fireLocator;
    [SerializeField] private Age _age;

    public Hand Hand => _hand;

    public Rigidbody2D Torso => _torso;

    public Transform SkeletonRoot => _skeletonRoot;

    public bool IsTouchingGround => _isTouchingGround;

    public Transform RootBone => _rootBone;

    public SpriteRenderer SpriteRenderer => _spriteRenderer;

    public Transform FireLocator => _fireLocator;

    public Age Age => _age;

    private List<BodyPart> _limbs;
    private bool _isInitialized;
    private ContactPoint2D[] _contactPoints;
    private ContactFilter2D _contactFilter;
    private Collider2D[] _overlapColliders;
    private bool _isTouchingGround;
    private Body _newBody;
    private bool _isTransitioning;
    private float _transitionFinishTime;

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
        _limbs = GetComponentsInChildren<BodyPart>().ToList();
        _isInitialized = true;
    }

    private void Update()
    {
        if (!_isInitialized)
        {
            return;
        }

        _isTouchingGround = _limbs.Any(limb =>
            limb.Rigidbody.GetContacts(_contactFilter, _contactPoints) > 0 ||
            limb.Rigidbody.OverlapCollider(_contactFilter, _overlapColliders) > 0);

        if (_isTransitioning && Time.time >= _transitionFinishTime)
        {
            _isTransitioning = false;
            _spriteRenderer.sprite = _newBody.SpriteRenderer.sprite;
            Destroy(_newBody.gameObject);
        }
    }

    public void StartTransition(Body newBody, bool fast = false)
    {
        _newBody = newBody;
        _newBody.gameObject.SetActive(false);
        _isTransitioning = true;

        var transitionDuration = fast ? 2f : _transitionDuration;

        _transitionFinishTime = Time.time + transitionDuration;


        foreach (var limb in _limbs)
        {
            var newBodyPart = _newBody.TryGetBodyPart(limb.name);

            if (newBodyPart == null)
            {
                continue;
            }

            limb.StartTransitioning(newBodyPart, transitionDuration);
        }
    }

    private BodyPart TryGetBodyPart(string bodyPartName)
    {
        if (_limbs == null)
        {
            _limbs = GetComponentsInChildren<BodyPart>().ToList();
        }

        return _limbs.FirstOrDefault(limb => limb.gameObject.name == bodyPartName);
    }

    private void BindSkinnedSprite()
    {
        var skinnedSprite = GetComponentInChildren<SpriteSkin>();

        if (skinnedSprite == null)
        {
            return;
        }
    }
}