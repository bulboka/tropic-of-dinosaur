using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkullManager : MonoBehaviour
{
    [SerializeField] private List<Skull> _skullPrefabs;
    [SerializeField] private float _skullSpawnPeriod;
    [SerializeField] private int _minAliveHeartsCount;
    [SerializeField] private Transform _suckerLocator;
    [SerializeField] private float _suckerForceMax;
    [SerializeField] private float _suckerForceAcceleration;
    [SerializeField] private GameObject _funnelSeal;
    [SerializeField] private float _suckerLocatorMinDistance;
    [SerializeField] private List<GameObject> _grounds;
    [SerializeField] private GameObject _suckerEffector;

    private int _nextSkullPrefabIndex;
    private float _nextSkullSpawnTime = -1;
    private List<Skull> _skulls = new();
    private List<ChickenHeart> _hearts;
    private int _aliveHeartsCount;
    private bool _isSucking;
    private float _suckerForce;

    public int NextSkullPrefabIndex => _nextSkullPrefabIndex;

    private void Update()
    {
        TrySpawnSkull();
    }

    private void FixedUpdateSuck()
    {
        if (!_isSucking)
        {
            return;
        }

        _suckerForce = Mathf.Min(_suckerForce + _suckerForceAcceleration, _suckerForceMax);

        var toSuckerVector = _suckerLocator.position - GameSession.Body.Torso.transform.position;

        GameSession.Body.Torso.AddForce(toSuckerVector.normalized * _suckerForce);
    }

    private void TrySpawnSkull()
    {
        if (_nextSkullSpawnTime < 0 || Time.time < _nextSkullSpawnTime)
        {
            return;
        }

        var newSkull = Instantiate(_skullPrefabs[NextSkullPrefabIndex]);
        _skulls.Add(newSkull);
        newSkull.OnPreyEaten += OnPreyEaten;
        newSkull.Initialize();

        _nextSkullPrefabIndex = NextSkullPrefabIndex + 1;

        if (NextSkullPrefabIndex < _skullPrefabs.Count)
        {
            _nextSkullSpawnTime = Time.time + _skullSpawnPeriod;
        }
        else
        {
            _nextSkullSpawnTime = -1;
        }
    }

    private void OnPreyEaten(Skull skull, Transform prey)
    {
        _aliveHeartsCount--;

        if (_aliveHeartsCount <= _minAliveHeartsCount)
        {
            FinishEating(skull);
        }
    }

    private void FinishEating(Skull lastSkull)
    {
        lastSkull.Follow();
        _skulls.Remove(lastSkull);

        foreach (var skull in _skulls)
        {
            if (skull != null)
            {
                skull.FlyAway();
            }
        }

        _skulls.Clear();

        _funnelSeal.SetActive(false);
        _suckerEffector.SetActive(true);
        _isSucking = true;
        _nextSkullSpawnTime = -1;

        var layer = LayerMask.NameToLayer("GroundSlipery");

        foreach (var ground in _grounds)
        {
            ground.layer = layer;
        }

        var hearts = FindObjectsByType<ChickenHeart>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        foreach (var heart in hearts)
        {
            heart.gameObject.layer = LayerMask.NameToLayer("ChickenHearts");
        }
    }

    public void SetTimer(float duration)
    {
        _nextSkullSpawnTime = Time.time + duration;

        if (_hearts == null)
        {
            _hearts = GameSession.ChickenHeartsManager.Hearts
                .Where(heart => heart.State == ChickenHeartState.FreeFollowing).ToList();
            _aliveHeartsCount = _hearts.Count;
        }
    }

    public Transform GetNextPrey()
    {
        if (_hearts.Count <= _minAliveHeartsCount)
        {
            return null;
        }

        var result = _hearts.First();
        _hearts.RemoveAt(0);
        return result.transform;
    }

    private void FixedUpdate()
    {
        FixedUpdateSuck();
    }

    public void StopSucking()
    {
        _isSucking = false;
    }
}