using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class RainbowTrigger : MonoBehaviour
{
    [SerializeField] private List<Transform> _collectibleLocators;
    [SerializeField] private Transform _handLocator;
    [SerializeField] private GameObject _sittingBody;
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private Transform _fireTarget;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private GameObject _slowdown;
    [SerializeField] private List<GameObject> _overlayEffects;
    [SerializeField] private float _overlayEffectPeriod;
    [SerializeField] private Vector2 _defaultGravity;

    private bool _isOverlaySwitching;
    private float _nextOverlaySwitchTime;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Body"))
        {
            return;
        }

        var locatorIndex = 0;

        foreach (var collectible in GameSession.StuckObjects)
        {
            collectible.GoToLocator(_collectibleLocators[locatorIndex]);
            locatorIndex++;
        }

        _collider.enabled = false;
        _sittingBody.SetActive(true);
        GameSession.Body.Hide();
        _sittingBody.SetActive(true);
        GameSession.Camera.SetTarget(_cameraTarget);
        GameSession.Camera.SmoothTime = 2.5f;

        _slowdown.SetActive(false);

        var handView = GameSession.Hand.View;
        var handViewParent = new GameObject();
        handViewParent.transform.position = handView.transform.parent.position;
        handViewParent.transform.rotation = handView.transform.parent.rotation;
        handView.transform.SetParent(handViewParent.transform);
        Destroy(GameSession.Hand.gameObject);
        var handDuration = Random.Range(3f, 6f);
        handViewParent.transform.DOMove(_handLocator.position, handDuration).SetEase(Ease.Linear);
        handViewParent.transform.DORotate(_handLocator.rotation.eulerAngles, handDuration).SetEase(Ease.Linear);

        var bodyFire = FindObjectOfType<BodyFire>();
        bodyFire.Target = _fireTarget;
        var bodyFireSprite = bodyFire.GetComponentInChildren<SpriteRenderer>();
        bodyFireSprite.sortingOrder = -1;
        _isOverlaySwitching = true;
        _nextOverlaySwitchTime = Time.time + _overlayEffectPeriod;

        var rollingFollower = FindObjectOfType<RollingFollower>();

        if (rollingFollower != null)
        {
            Destroy(rollingFollower.gameObject);
        }

        GameSession.IsGameOver = true;

        foreach (var heart in FindObjectsOfType<ChickenHeart>())
        {
            Destroy(heart.gameObject);
        }

        foreach (var skull in FindObjectsOfType<Skull>())
        {
            Destroy(skull.gameObject);
        }

        Physics2D.gravity = _defaultGravity;
    }

    private void Update()
    {
        if (!_isOverlaySwitching || Time.time < _nextOverlaySwitchTime)
        {
            return;
        }

        _nextOverlaySwitchTime = Time.time + _overlayEffectPeriod;
        GameSession.Camera.SwitchOverlayEffect(_overlayEffects[Random.Range(0, _overlayEffects.Count)]);
    }
}