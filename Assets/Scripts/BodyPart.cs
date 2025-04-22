#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [SerializeField] private HingeJoint2D _hingeJoint;
    [SerializeField] private CapsuleCollider2D _collider;
    [SerializeField] private Rigidbody2D _rigidbody;

    private bool _isTransitioning;
    private float _transitionStartTime;
    private float _transitionDuration;

    private Vector2 _targetColliderSize;
    private Vector2 _targetColliderOffset;
    private Vector2 _targetJointConnectedAnchor;
    private Vector2 _targetJointAnchor;

    private Vector2 _initialColliderSize;
    private Vector2 _initialColliderOffset;
    private Vector2 _initialJointConnectedAnchor;
    private Vector2 _initialJointAnchor;

    public HingeJoint2D HingeJoint => _hingeJoint;

    public CapsuleCollider2D Collider => _collider;

    public Rigidbody2D Rigidbody => _rigidbody;

    private void Update()
    {
        UpdateTransition();
    }

    private void UpdateTransition()
    {
        if (!_isTransitioning)
        {
            return;
        }

        var factor = (Time.time - _transitionStartTime) / _transitionDuration;

        if (factor >= 1f)
        {
            _isTransitioning = false;
        }

        factor = Mathf.Clamp01(factor);

        if (_collider != null)
        {
            _collider.offset = Vector2.Lerp(_initialColliderOffset, _targetColliderOffset, factor);
            _collider.size = Vector2.Lerp(_initialColliderSize, _targetColliderSize, factor);
        }

        if (_hingeJoint != null)
        {
            _hingeJoint.anchor = Vector2.Lerp(_initialJointAnchor, _targetJointAnchor, factor);
            _hingeJoint.connectedAnchor = Vector2.Lerp(_initialJointConnectedAnchor, _targetJointConnectedAnchor, factor);
        }
    }

    public void StartTransitioning(BodyPart targetBodyPart, float duration)
    {
        _isTransitioning = true;
        _transitionStartTime = Time.time;
        _transitionDuration = duration;

        if (_collider != null && targetBodyPart.Collider != null)
        {
            _targetColliderOffset = targetBodyPart.Collider.offset;
            _targetColliderSize = targetBodyPart.Collider.size;
            _initialColliderOffset = _collider.offset;
            _initialColliderSize = _collider.size;
        }

        if (_hingeJoint != null && targetBodyPart.HingeJoint != null)
        {
            _initialJointAnchor = _hingeJoint.anchor;
            _initialJointConnectedAnchor = _hingeJoint.connectedAnchor;
            _targetJointAnchor = targetBodyPart.HingeJoint.anchor;
            _targetJointConnectedAnchor = targetBodyPart.HingeJoint.connectedAnchor;
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_rigidbody != null)
        {
            return;
        }

        AutoConfig();
    }

    public void AutoConfig()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _hingeJoint = GetComponent<HingeJoint2D>();
        _collider = GetComponent<CapsuleCollider2D>();

        EditorUtility.SetDirty(gameObject);
        AssetDatabase.SaveAssetIfDirty(gameObject);
    }
#endif

    public void DestroyPhysics()
    {
        if (_hingeJoint != null)
        {
            Destroy(_hingeJoint);
            _hingeJoint = null;
        }

        if (_rigidbody != null)
        {
            Destroy(_rigidbody);
            _rigidbody = null;
        }

        if (_collider != null)
        {
            Destroy(_collider);
            _collider = null;
        }
    }
}