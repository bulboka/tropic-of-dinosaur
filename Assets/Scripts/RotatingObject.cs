using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    [SerializeField] private float _speed;

    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, _speed * Time.deltaTime));
    }
}
