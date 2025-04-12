using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChickenHeartsManager : MonoBehaviour
{
    [SerializeField] private float _maxFreeFollowDistance;

    private List<ChickenHeart> _hearts;

    public List<ChickenHeart> Hearts => _hearts;

    public void Initialize()
    {
        _hearts = FindObjectsByType<ChickenHeart>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
    }

    public void SummonHeartsTo(ChickenHeartsTrigger chickenHeartsTrigger)
    {
        foreach (var heart in Hearts.Where(heart => heart.State == ChickenHeartState.Awake))
        {
            heart.GoToTrigger(chickenHeartsTrigger);
        }
    }

    public void FreeHearts()
    {
        var maxFreeFollowDistanceSqr = Mathf.Pow(_maxFreeFollowDistance, 2);

        for (var i = Hearts.Count - 1; i >= 0; i--)
        {
            var heart = Hearts[i];

            if (heart.State == ChickenHeartState.Awake &&
                (heart.transform.position - GameSession.Body.Torso.transform.position).sqrMagnitude <= maxFreeFollowDistanceSqr)
            {
                heart.State = ChickenHeartState.FreeFollowing;
                heart.gameObject.layer = LayerMask.NameToLayer("Ground");
            }
            else
            {
                Destroy(heart.gameObject);
                Hearts.RemoveAt(i);
            }
        }
    }
}