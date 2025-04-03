using UnityEngine;
using Random = UnityEngine.Random;

public class MapleHelicopter : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _rotationSpeedMin;
    [SerializeField] private float _rotationSpeedMax;
    [SerializeField] private float _zMin;
    [SerializeField] private float _zMax;

    private float _rotationSpeed;

    private void Start()
    {
        _rotationSpeed = Random.Range(_rotationSpeedMin, _rotationSpeedMax);

        if (Random.value > 0.5f)
        {
            _rotationSpeed *= -1f;
        }

        if (Random.value > 0.5f)
        {
            transform.localScale =
                new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, Random.Range(_zMin, _zMax));
        _spriteRenderer.sortingOrder = transform.position.z > 0 ? -4 : 5;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360f)));
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, _rotationSpeed * Time.deltaTime));
    }
}