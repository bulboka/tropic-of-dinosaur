using UnityEngine;

public class RotatingBody : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _rotationSpeed;

    private void Update()
    {
        _rigidbody.rotation += _rotationSpeed * Time.deltaTime;
    }
}