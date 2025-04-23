using System.Linq;
using DG.Tweening;
using UnityEngine;

public class StickyObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private float _unstickShift;
    [SerializeField] private float _unstickForce;

    private bool _isStuck;
    private FixedJoint2D _stickJoint;
    private Vector3 _contactPointLocal;
    private Vector3 _collisionNormalLocal;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_isStuck || (!other.gameObject.CompareTag("Body") && !other.gameObject.CompareTag("StuckObject")))
        {
            return;
        }

        _isStuck = true;

        var existingJoints = GetComponents<Joint2D>();

        foreach (var joint in existingJoints)
        {
            Destroy(joint);
        }

        var randomTorque = GetComponent<RandomTorque>();

        if (randomTorque != null)
        {
            Destroy(randomTorque);
        }

        _rigidbody.bodyType = RigidbodyType2D.Dynamic;

        _stickJoint = gameObject.AddComponent<FixedJoint2D>();
        _stickJoint.connectedBody = other.rigidbody;

        var contact = other.contacts.FirstOrDefault();
        _contactPointLocal = transform.InverseTransformPoint(contact.point);
        _collisionNormalLocal = transform.InverseTransformDirection(contact.normal).normalized;

        GameSession.StuckObjects.Add(this);
        _collider.enabled = false;

        gameObject.layer = LayerMask.NameToLayer("StuckObject");
        //gameObject.tag = "StuckObject";

        OnStuck();
    }

    protected virtual void OnStuck()
    {}

    public void Unstick()
    {
        Destroy(_stickJoint);
        _stickJoint = null;
        _isStuck = false;
        gameObject.layer = LayerMask.NameToLayer("Default");

        var collisionNormalWorld = transform.TransformDirection(_collisionNormalLocal);
        _rigidbody.MovePosition(_rigidbody.position + (Vector2)collisionNormalWorld * _unstickShift);
        _rigidbody.AddForce(collisionNormalWorld * _unstickForce);
    }

    public void GoToLocator(Transform locator)
    {
        Destroy(_stickJoint);
        _stickJoint = null;
        _isStuck = false;
        Destroy(_collider);
        _rigidbody.bodyType = RigidbodyType2D.Kinematic;

        transform
            .DOMove(locator.position, Random.Range(3f, 6f))
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                _rigidbody.MovePosition(transform.position);
            });
    }
}
