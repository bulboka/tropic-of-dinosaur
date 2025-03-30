using System;
using UnityEngine;

public class Korka : MonoBehaviour
{
    [SerializeField] private float _hidingSpeed;

    private bool _isHiding;
    private float _scaleMult = 1f;
    private Vector3 _initialScale;

    private void Start()
    {
        _initialScale = transform.localScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Fire"))
        {
            _isHiding = true;
        }
    }

    private void Update()
    {
        if (_isHiding)
        {
            _scaleMult -= _hidingSpeed * Time.deltaTime;

            if (_scaleMult <= 0)
            {
                Destroy(gameObject);
                return;
            }

            transform.localScale = _initialScale * _scaleMult;
        }
    }
}
