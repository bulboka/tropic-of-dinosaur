using System;
using UnityEngine;

public class FixedRotationBody : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Transform _target;

    private void Update()
    {
        _rigidbody.MovePosition(_target.position);
    }

    private void OnDrawGizmos()
    {
        if (_target == null)
        {
            return;
        }

        var view = GetComponentInChildren<SpriteRenderer>();

        if (view == null)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(view.transform.position, _target.position);
    }
}