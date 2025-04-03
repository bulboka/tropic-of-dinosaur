using System;
using UnityEngine;

public class RotatingBodySpeedTrigger : MonoBehaviour
{
    [SerializeField] private float _slowSpeed;
    [SerializeField] private float _fastSpeed;
    [SerializeField] private RotatingBody _rotatingBody;

    private void OnTriggerEnter2D(Collider2D other)
    {
        _rotatingBody.RotationSpeed = _slowSpeed;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _rotatingBody.RotationSpeed = _fastSpeed;
    }
}