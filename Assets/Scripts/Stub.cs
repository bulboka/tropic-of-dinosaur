using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Stub : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Collider2D _rigidCollider;
    [SerializeField] private Collider2D _mainStarCollider;
    [SerializeField] private float _cameraZoomAddOnActivate;
    [SerializeField] private float _handForceAddOnActivate;
    [SerializeField] private float _cameraZoomMax;
    [SerializeField] private float _rotationSpeedMin;
    [SerializeField] private float _rotationSpeedMax;
    [SerializeField] private RotatingObject _rotatingObject;
    [SerializeField] private LineRenderer _lineRendererPrefab;
    [SerializeField] private List<Color> _lineColors;
    [SerializeField] private GameObject _activeViewPrefab;
    [SerializeField] private float _lineWidthMin;
    [SerializeField] private float _lineWidthMax;
    [SerializeField] private float _minorStarScale;

    private bool _isMainStar;
    private MainStarLocator _mainStarLocator;
    private bool _isStarActive;
    private List<LineRenderer> _lineRenderers = new();
    private GameObject _activeView;

    public bool IsStarActive => _isStarActive;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _rigidbody.rotation = Random.Range(0, 360f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryTurnToStar(other);
        TryActivateMainStar(other);
    }

    private void TryActivateMainStar(Collider2D other)
    {
        if (!_isMainStar || !other.gameObject.CompareTag("Body") || other.gameObject.name != "Torso")
        {
            return;
        }

        ActivateStar(true, true, true);
    }

    private void TryTurnToStar(Collider2D other)
    {
        if (!other.gameObject.CompareTag("StubToStarTrigger"))
        {
            return;
        }

        var constellationGO = GameObject.FindGameObjectWithTag("Constellation");

        if (constellationGO == null)
        {
            return;
        }

        var constellation = constellationGO.GetComponent<Constellation>();

        if (constellation == null)
        {
            return;
        }

        Destroy(_rigidbody);
        _rigidCollider.enabled = false;

        _rotatingObject.enabled = true;
        _rotatingObject.Speed =
            Random.Range(_rotationSpeedMin, _rotationSpeedMax) * (Random.value <= 0.5f ? -1f : 1f);

        _mainStarLocator = constellation.GetNextMainStarLocator();

        if (_mainStarLocator != null)
        {
            gameObject.layer = LayerMask.NameToLayer("Trigger");
            _isMainStar = true;
            transform.position = _mainStarLocator.transform.position;
            _mainStarCollider.enabled = true;
            _mainStarLocator.AttachedStub = this;

            return;
        }

        var startLocator = constellation.GetNextStarLocator();

        if (startLocator != null)
        {
            transform.position = startLocator.position;
            transform.localScale = Vector3.one * _minorStarScale;
            return;
        }

        Destroy(gameObject);
    }

    public void ActivateStar(bool updateCamera, bool updateHand, bool dispatchEvents)
    {
        if (_isStarActive)
        {
            return;
        }

        _mainStarCollider.enabled = false;
        _isStarActive = true;

        _activeView = Instantiate(_activeViewPrefab, transform);
        _activeView.GetComponentInChildren<SpriteRenderer>().color = _lineColors[Random.Range(0, _lineColors.Count)];

        foreach (var connectedLocator in _mainStarLocator.ConnectedLocators)
        {
            if (connectedLocator.AttachedStub != null && connectedLocator.AttachedStub.IsStarActive)
            {
                var lineRenderer = Instantiate(_lineRendererPrefab, transform);
                lineRenderer.transform.localPosition = Vector3.zero;

                var positions = new[] { transform.position, connectedLocator.AttachedStub.transform.position };
                lineRenderer.positionCount = 2;
                lineRenderer.SetPositions(positions);
                //lineRenderer.startColor = lineRenderer.endColor = _lineColors[Random.Range(0, _lineColors.Count)];

                var randomColor = _lineColors[Random.Range(0, _lineColors.Count)];
                var gradient = lineRenderer.colorGradient;

                for (var i = 0; i < gradient.colorKeys.Length; i++)
                {
                    var colorKey = gradient.colorKeys[i];
                    colorKey.color = randomColor;
                    gradient.colorKeys[i] = colorKey;
                }

                lineRenderer.colorGradient = gradient;

                lineRenderer.widthMultiplier = Random.Range(_lineWidthMin, _lineWidthMax);

                _lineRenderers.Add(lineRenderer);
            }
        }

        if (updateCamera)
        {
            GameSession.Camera.Zoom -= _cameraZoomAddOnActivate;

            if (GameSession.Camera.Zoom < _cameraZoomMax)
            {
                GameSession.Camera.Zoom = _cameraZoomMax;
            }
        }

        if (updateHand)
        {
            GameSession.Hand.MinForce += _handForceAddOnActivate;
            GameSession.Hand.MaxForce += _handForceAddOnActivate;
        }

        if (dispatchEvents)
        {
            _mainStarLocator.OnActivated?.Invoke();
        }
    }
}