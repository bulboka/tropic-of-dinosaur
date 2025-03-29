using UnityEngine;

public class StickyObject : MonoBehaviour
{
    private bool _isStuck;
    private void Start()
    {

    }

    private void Update()
    {
        
    }

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

        var stickJoint = gameObject.AddComponent<FixedJoint2D>();
        stickJoint.connectedBody = other.rigidbody;

        gameObject.tag = "StuckObject";
    }
}
