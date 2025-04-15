using System;
using UnityEngine;

public class DarknessTrigger : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private float _hidingSpeed;

    private bool _isHiding;
    private float _alpha = 1f;

    private void Start()
    {
        _spriteRenderer.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Body"))
        {
            return;
        }

        _collider.enabled = false;
        _isHiding = true;
    }

    private void Update()
    {
        if (!_isHiding)
        {
            return;
        }

        _alpha -= _hidingSpeed * Time.deltaTime;

        if (_alpha <= 0)
        {
            _alpha = 0;
            _isHiding = false;
        }

        var color = _spriteRenderer.color;
        color.a = _alpha;
        _spriteRenderer.color = color;
    }
}