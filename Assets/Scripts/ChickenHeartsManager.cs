using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChickenHeartsManager : MonoBehaviour
{
    [SerializeField] private float _maxFreeFollowDistance;
    [SerializeField] private int _specialHeartsCount;

    private List<ChickenHeart> _hearts;
    private List<ChickenHeart> _specialHearts = new();

    public List<ChickenHeart> Hearts => _hearts;

    public void Initialize()
    {
        _hearts = FindObjectsByType<ChickenHeart>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();

        foreach (var heart in _hearts)
        {
            heart.OnStateChange += OnHeartStateChange;
        }
    }

    private void OnHeartStateChange(ChickenHeart heart)
    {
        if (heart.State != ChickenHeartState.Awake)
        {
            return;
        }

        for (var i = _specialHearts.Count - 1; i >= 0; i--)
        {
            var specialHeart = _specialHearts[i];

            if ((specialHeart.transform.position - GameSession.Body.Torso.transform.position).sqrMagnitude >
                _maxFreeFollowDistance * _maxFreeFollowDistance)
            {
                _specialHearts.RemoveAt(i);
            }
        }

        if (_specialHearts.Count < _specialHeartsCount)
        {
            _specialHearts.Add(heart);
            heart.BecomeSpecial();
        }
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
        var maxFreeFollowDistanceSqr = Mathf.Pow(_maxFreeFollowDistance, 2);
        var heartsToDestroy = new List<ChickenHeart>();

        for (var i = _hearts.Count - 1; i >= 0; i--)
        {
            var heart = _hearts[i];

            if (heart.State == ChickenHeartState.Awake &&
                (heart.transform.position - GameSession.Body.Torso.transform.position).sqrMagnitude <= maxFreeFollowDistanceSqr)
            {
                heart.State = ChickenHeartState.FreeFollowing;
                heart.gameObject.layer = LayerMask.NameToLayer("Ground");
            }
            else
            {
                heartsToDestroy.Add(heart);
                _hearts.RemoveAt(i);
            }
        }

        foreach (var chickenHeart in heartsToDestroy)
        {
            chickenHeart.OnStateChange -= OnHeartStateChange;
            Destroy(chickenHeart.gameObject);
        }
    }
}