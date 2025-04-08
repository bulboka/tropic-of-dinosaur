using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChickenHeartsManager : MonoBehaviour
{
    private List<ChickenHeart> _hearts;
    public void Initialize()
    {
        _hearts = FindObjectsByType<ChickenHeart>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
    }

    public void SummonHeartsTo(ChickenHeartsTrigger chickenHeartsTrigger)
    {
        foreach (var heart in _hearts.Where(heart => heart.State == ChickenHeartState.Awake))
        {
            heart.GoToTrigger(chickenHeartsTrigger);
        }
    }

    public void FreeHearts()
    {
        foreach (var heart in _hearts.Where(heart => heart.State == ChickenHeartState.Awake))
        {
            heart.State = ChickenHeartState.FreeFollowing;
            heart.gameObject.layer = LayerMask.NameToLayer("Ground");
        }
    }
}