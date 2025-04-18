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
    private HandView _view;
    private Body _body;

    public bool IsInputEnabled
    {
        get => _isInputEnabled;
        set => _isInputEnabled = value;
    }

    public HandView View => _view;

    public float MouseSensitivity
    {
        get => _mouseSensitivity;
        set => _mouseSensitivity = value;
    }

    public float MaxForce
    {
        get => _maxForce;
        set => _maxForce = value;
    }

    public float MinForce
    {
        get => _minForce;
        set => _minForce = value;
    }

    public float AirbornForceMult
    {
        get => _airbornForceMult;
        set => _airbornForceMult = value;
    }

    public void Initialize(Body body)
    {
        _body = body;
    }

    private void Start()
    {
        _desiredPosition = transform.position;
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

        var inputX = Input.GetAxisRaw("Mouse X") * MouseSensitivity;
        var inputY = Input.GetAxisRaw("Mouse Y") * MouseSensitivity;

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
                    ((MinForce + (MaxForce - MinForce) * Mathf.Clamp01(positionDeltaMag / _maxPositioDelta)) *
                     (_body.IsTouchingGround ? 1f : AirbornForceMult));

        _rigidbody.AddForce(force);
    }

    public void SetView(HandView view)
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

    public void CopyParamsFrom(Hand hand)
    {
        _minForce = hand.MinForce;
        _maxForce = hand.MaxForce;
        _airbornForceMult = hand.AirbornForceMult;
    }
}
