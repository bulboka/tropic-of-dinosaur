using System.Linq;
using UnityEngine;

public class StickyObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
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

        _stickJoint = gameObject.AddComponent<FixedJoint2D>();
        _stickJoint.connectedBody = other.rigidbody;

        var contact = other.contacts.FirstOrDefault();
        _contactPointLocal = transform.InverseTransformPoint(contact.point);
        _collisionNormalLocal = transform.InverseTransformDirection(contact.normal).normalized;

        GameSession.StuckObjects.Add(this);

        gameObject.layer = LayerMask.NameToLayer("StuckObject");
        //gameObject.tag = "StuckObject";
    }

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
}
