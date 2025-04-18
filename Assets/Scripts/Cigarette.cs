using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Cigarette : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _smokeAnimationDuration;
    [SerializeField] private float _moveDuration;
    [SerializeField] private RotatingObject _rotatingObject;
    [SerializeField] private CigaretteSmoke _smokePrefab;
    [SerializeField] private float _smokeSpawnPeriod;

    private Transform _finishLocator;
    private bool _isSmoking;
    private float _smokeEndTime;
    private List<Transform> _smokePath;
    private float _nextSmokeSpawnTime;

    public void Initialize(Transform startLocator, Transform finishLocator, List<Transform> smokePath)
    {
        _finishLocator = finishLocator;
        _smokePath = smokePath;

        transform.position = startLocator.position;
        transform.rotation = startLocator.rotation;
        transform.SetParent(startLocator, true);
        _isSmoking = true;
        _smokeEndTime = Time.time + _smokeAnimationDuration;
        _nextSmokeSpawnTime = Time.time + _smokeSpawnPeriod;
    }

    private void Update()
    {
        if (!_isSmoking)
        {
            return;
        }

        if (Time.time >= _smokeEndTime)
        {
            _isSmoking = false;
            _rotatingObject.enabled = true;
            transform.SetParent(null, true);

            transform
                .DOMove(_finishLocator.position, _moveDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() => { Destroy(gameObject); });

            return;
        }

        if (Time.time >= _nextSmokeSpawnTime)
        {
            _nextSmokeSpawnTime = Time.time + _smokeSpawnPeriod;

            var smoke = Instantiate(_smokePrefab);
            smoke.Initialize(_smokePath);
        }
    }
}