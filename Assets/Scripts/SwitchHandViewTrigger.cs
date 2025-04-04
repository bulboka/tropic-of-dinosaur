using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwitchHandViewTrigger : MonoBehaviour
{
    [SerializeField] private List<GameObject> _handViewPrefabs;

    private void OnTriggerEnter2D(Collider2D other)
    {
        gameObject.SetActive(false);

        var suitableViews = _handViewPrefabs.Where(viewPrefab => !GameSession.UsedHandViewPrefabs.Contains(viewPrefab))
            .ToList();

        if (!suitableViews.Any())
        {
            return;
        }

        var newViewPrefab = suitableViews[Random.Range(0, suitableViews.Count)];
        var newView = Instantiate(newViewPrefab);
        GameSession.UsedHandViewPrefabs.Add(newViewPrefab);
        GameSession.Hand.SetView(newView);
    }
}