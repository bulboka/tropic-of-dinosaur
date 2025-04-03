using UnityEngine;

public class RandomAreaEffector : MonoBehaviour
{
    [SerializeField] private float _angleMin;
    [SerializeField] private float _angleMax;
    [SerializeField] private float _periodDurationMin;
    [SerializeField] private float _periodDurationMax;
    [SerializeField] private AreaEffector2D _areaEffector;

    private float _nextChangeTime;

    private void Update()
    {
        if (Time.time < _nextChangeTime)
        {
            return;
        }

        _nextChangeTime = Time.time + Random.Range(_periodDurationMin, _periodDurationMax);
        _areaEffector.forceAngle = Random.Range(_angleMin, _angleMax);
        Debug.Log($"New angle:{_areaEffector.forceAngle}");
    }
}