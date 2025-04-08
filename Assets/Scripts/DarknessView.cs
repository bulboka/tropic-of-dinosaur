using UnityEngine;

public class DarknessView : MonoBehaviour
{
    [SerializeField] private float _minPerionDuration;
    [SerializeField] private float _maxPerionDuration;

    private float _nextChangeTime;

    private void Update()
    {
        if (Time.time < _nextChangeTime)
        {
            return;
        }

        _nextChangeTime = Time.time + Random.Range(_minPerionDuration, _maxPerionDuration);

        transform.Rotate(new Vector3(0, 0, Random.Range(1, 4) * 90f));
    }
}