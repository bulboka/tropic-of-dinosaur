using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Constellation : MonoBehaviour
{
    [SerializeField] private Transform _starLocatorsContainer;
    [SerializeField] private Transform _mainStarLocatorsContainer;
    [SerializeField] private float _finishCameraZoom;
    [SerializeField] private float _finishCameraZoomTime;
    [SerializeField] private float _delayBeforeLeave;
    [SerializeField] private float _leaveCameraZoom;
    [SerializeField] private float _leaveCameraZoomTime;
    [SerializeField] private PointEffector2D _leaveEffector;
    [SerializeField] private GameObject _ground;
    [SerializeField] private GameObject _leaveSeal;
    [SerializeField] private float _graceTimerDuration;
    [SerializeField] private int _maxStarsLeftNotActivated;
    [SerializeField] private GameObject _stegosaurus;
    [SerializeField] private float _leaveEffectorStartForce;
    [SerializeField] private float _leaveEffectorMaxForce;
    [SerializeField] private float _leaveEffectorForceSpeed;

    private int _nextStarLocatorIndex;
    private int _nextMainStarLocatorIndex;
    private List<MainStarLocator> _mainStarLocators = new();
    private int _activatedStarsCount;
    private float _leaveTime = -1;
    private float _graceTimerEnd = -1;

    private void Start()
    {
        _mainStarLocators = _mainStarLocatorsContainer.GetComponentsInChildren<MainStarLocator>().ToList();

        foreach (var starLocator in _mainStarLocators)
        {
            starLocator.OnActivated += OnStarActivated;

            foreach (var connectedLocator in starLocator.ConnectedLocators)
            {
                if (!connectedLocator.ConnectedLocators.Contains(starLocator))
                {
                    connectedLocator.ConnectedLocators.Add(starLocator);
                }
            }
        }
    }

    private void OnStarActivated()
    {
        _activatedStarsCount++;

        if (_activatedStarsCount >= _mainStarLocators.Count)
        {
            StartFinish();
            return;
        }

        if (_mainStarLocators.Count - _activatedStarsCount <= _maxStarsLeftNotActivated && _graceTimerEnd < 0)
        {
            _graceTimerEnd = Time.time + _graceTimerDuration;
        }
    }

    private void StartFinish()
    {
        _stegosaurus.SetActive(true);
        GameSession.Camera.Zoom = _finishCameraZoom;
        GameSession.Camera.ZoomSmoothTime = _finishCameraZoomTime;
        _leaveTime = Time.time + _delayBeforeLeave;
        _graceTimerEnd = -1;
    }

    private void Update()
    {
        UpdateLeaveTimer();
        UpdateGraceTimer();
        UpdateLeaveEffector();
    }

    private void UpdateLeaveEffector()
    {
        if (!_leaveEffector.gameObject.activeSelf || _leaveEffector.forceMagnitude <= _leaveEffectorMaxForce)
        {
            return;
        }

        _leaveEffector.forceMagnitude += _leaveEffectorForceSpeed * Time.deltaTime;
    }

    private void UpdateGraceTimer()
    {
        if (_graceTimerEnd < 0 || Time.time < _graceTimerEnd)
        {
            return;
        }

        foreach (var locator in _mainStarLocators)
        {
            locator.AttachedStub.ActivateStar(true, true, true);
        }
        //StartFinish();
    }

    private void UpdateLeaveTimer()
    {
        if (_leaveTime < 0 || Time.time < _leaveTime)
        {
            return;
        }

        _leaveTime = -1;
        GameSession.Camera.Zoom = _leaveCameraZoom;
        GameSession.Camera.ZoomSmoothTime = _leaveCameraZoomTime;
        _leaveEffector.gameObject.SetActive(true);
        _leaveEffector.forceMagnitude = _leaveEffectorStartForce;
        _leaveSeal.SetActive(false);
        _ground.SetActive(false);
    }

    public Transform GetNextStarLocator()
    {
        if (_nextStarLocatorIndex >= _starLocatorsContainer.childCount)
        {
            return null;
        }

        return _starLocatorsContainer.GetChild(_nextStarLocatorIndex++);
    }

    public MainStarLocator GetNextMainStarLocator()
    {
        if (_nextMainStarLocatorIndex >= _mainStarLocators.Count)
        {
            return null;
        }

        return _mainStarLocators[_nextMainStarLocatorIndex++];
    }
}