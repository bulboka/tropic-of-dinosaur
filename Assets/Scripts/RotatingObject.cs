using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    [SerializeField] private float _speed;

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, _speed * Time.deltaTime));
    }
}
