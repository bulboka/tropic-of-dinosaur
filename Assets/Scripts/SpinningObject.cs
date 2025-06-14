using UnityEngine;

public class SpinningObject : MonoBehaviour
{
    [SerializeField] private float _speed;

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, _speed * Time.deltaTime));
    }
}