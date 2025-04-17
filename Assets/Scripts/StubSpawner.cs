using System.Runtime.CompilerServices;
using UnityEngine;

public class StubSpawner : MonoBehaviour
{
    [SerializeField] private Vector2 _size = new(1f, 1f);
    [SerializeField] private float _distBetweenStubs;
    [SerializeField] private Stub _stubPrefab;
    [SerializeField] private int _maxCount;

    private void Start()
    {
        SpawnStubs();
    }

    public void SpawnStubs()
    {
        var spawnedCount = 0;

        for (var i = 0; i < Mathf.Floor(_size.y / _distBetweenStubs); i++)
        {
            for (var j = 0; j < Mathf.Floor(_size.x / _distBetweenStubs); j++)
            {
                var stub = Instantiate(_stubPrefab, transform);

                stub.transform.localPosition = new Vector3(
                    -_size.x * 0.5f + _distBetweenStubs * j +
                    Random.Range(-_distBetweenStubs * 0.3f, _distBetweenStubs * 0.3f),
                    -_size.y * 0.5f + _distBetweenStubs * i +
                    Random.Range(-_distBetweenStubs * 0.3f, _distBetweenStubs * 0.3f), 0);

                spawnedCount++;

                if (spawnedCount >= _maxCount)
                {
                    return;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, _size);
    }
}