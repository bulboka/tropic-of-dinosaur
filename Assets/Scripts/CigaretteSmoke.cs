using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class CigaretteSmoke : MonoBehaviour
{
    [SerializeField] private float _moveDuration;
    [SerializeField] private float _randomRadius;

    public void Initialize(List<Transform> path)
    {
        transform.position = path.First().position;

        var pathPoints = new Vector3[path.Count];

        for (var i = 0; i < path.Count; i++)
        {
            pathPoints[i] = path[i].position;
            pathPoints[i].x += Random.Range(-_randomRadius, _randomRadius);
            pathPoints[i].y += Random.Range(-_randomRadius, _randomRadius);
        }

        transform
            .DOPath(pathPoints, _moveDuration, PathType.CatmullRom)
            .SetEase(Ease.Linear)
            .OnComplete(() => Destroy(gameObject));

        transform.localScale = Vector3.zero;

        transform.DOScale(Vector3.one, 1.5f);
    }
}