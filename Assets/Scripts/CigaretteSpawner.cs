using System.Collections.Generic;
using UnityEngine;

public class CigaretteSpawner : MonoBehaviour
{
    [SerializeField] private Cigarette _cigarettePrefab;
    [SerializeField] private Transform _cigaretteStartLocator;
    [SerializeField] private Transform _cigaretteFinishLocator;
    [SerializeField] private float _cigaretteSpawnPeriod;
    [SerializeField] private List<Transform> _smokePath;

    private float nextCigaretteSpawnTime;

    private void Update()
    {
        if (Time.time < nextCigaretteSpawnTime)
        {
            return;
        }

        nextCigaretteSpawnTime = Time.time + _cigaretteSpawnPeriod;

        var cigarette = Instantiate(_cigarettePrefab);
        cigarette.Initialize(_cigaretteStartLocator, _cigaretteFinishLocator, _smokePath);
    }
}