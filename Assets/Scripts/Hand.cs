using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _mouseSensitivity;
    [SerializeField] private float _maxForce;
    [SerializeField] private float _minForce;
    [SerializeField] private float _maxPositioDelta;
    [SerializeField] private float _maxdistanceFromHand;
    [SerializeField] private Transform _viewContainer;
    [SerializeField] private Transform _target;
    [SerializeField] private float _maxDistanceFromCamera;
    [SerializeField] private float _airbornForceMult = 1f;

    private Vector2 _desiredVelocity;
    private Vector3 _desiredPosition;
    private Vector3 _currentInput;
    private bool _isInputEnabled;
    private GameObject _view;
    private Body _body;

    public bool IsInputEnabled
    {
        get => _isInputEnabled;
        set => _isInputEnabled = value;
    }

    public GameObject View => _view;

    public void Initialize(Body body)
    {
        _body = body;
    }

    private void Start()
    {
        _desiredPosition = transform.position;

#if UNITY_EDITOR

#else
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Screen.SetResolution(1600, 1200, FullScreenMode.FullScreenWindow);
#endif
    }

    private void Update()
    {
        UpdateInput();
        UpdateViewRotation();
    }

    private void UpdateViewRotation()
    {
        var toTargetVector = _target.position - _viewContainer.position;
        toTargetVector.z = 0;
        _viewContainer.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(toTargetVector.y, toTargetVector.x) * Mathf.Rad2Deg);
    }

    private void UpdateInput()
    {
        if (!_isInputEnabled)
        {
            return;
        }

        var inputX = Input.GetAxisRaw("Mouse X") * _mouseSensitivity;
        var inputY = Input.GetAxisRaw("Mouse Y") * _mouseSensitivity;

        _desiredPosition = new Vector3(_desiredPosition.x + inputX, _desiredPosition.y + inputY, 0);

        var vectorFromHand = _desiredPosition - transform.position;
        var distanceFromHand = vectorFromHand.magnitude;

        if (distanceFromHand > _maxdistanceFromHand)
        {
            _desiredPosition = transform.position + vectorFromHand.normalized * _maxdistanceFromHand;
        }

        var vectorFromCamera = _desiredPosition - GameSession.Camera.transform.position;
        var distanceFromCamera = vectorFromCamera.magnitude;

        if (distanceFromCamera > _maxDistanceFromCamera)
        {
            _desiredPosition = GameSession.Camera.transform.position + vectorFromCamera.normalized * _maxDistanceFromCamera;
        }
    }

    private void FixedUpdate()
    {
        UpdateByDesiredPosition();
    }

    private void UpdateByDesiredPosition()
    {
        var positionDelta = _desiredPosition - transform.position;
        positionDelta.z = 0;
        var positionDeltaMag = positionDelta.magnitude;
        var positionDeltaNormalized = positionDelta.normalized;

        var force = positionDeltaNormalized *
                    ((_minForce + (_maxForce - _minForce) * Mathf.Clamp01(positionDeltaMag / _maxPositioDelta)) *
                     (_body.IsTouchingGround ? 1f : _airbornForceMult));

        _rigidbody.AddForce(force);
    }

    public void SetView(GameObject view)
    {
        if (_view != null)
        {
            Destroy(_view.gameObject);
        }

        _view = view;
        _view.transform.SetParent(_viewContainer);
        _view.transform.localPosition = Vector3.zero;
        _view.transform.localRotation = Quaternion.identity;
        _view.transform.localScale = Vector3.one;
    }

    public void Dispose()
    {
        _view = null;
    }
}
