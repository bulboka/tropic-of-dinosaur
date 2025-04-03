using UnityEngine;

public class RotatingBody : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _rotationSpeed;

    public float RotationSpeed
    {
        get => _rotationSpeed;
        set => _rotationSpeed = value;
    }

    private void Update()
    {
        _rigidbody.rotation += RotationSpeed * Time.deltaTime;
    }
}