using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _smoothTime;
    [SerializeField] private float _zoomSmoothTime;
    [SerializeField] private float _margin;
    [SerializeField] private Transform _overlayEffectContainer;
    [SerializeField] private Camera _camera;

    private Vector3 _velocity;
    private Vector3 _innerVelocity;
    private float _maxX;
    private GameObject _overlayEffect;
    private Vector3 _shift;
    private List<CameraShiftArea> _shiftAreas;
    private float _zoom;

    public Camera Camera => _camera;

    public float Zoom
    {
        get => _zoom;
        set => _zoom = value;
    }

    public Vector3 Shift
    {
        get => _shift;
        set => _shift = value;
    }

    public float ZoomSmoothTime
    {
        get => _zoomSmoothTime;
        set => _zoomSmoothTime = value;
    }

    private void Start()
    {
        _maxX = transform.position.x;

        if (_overlayEffectContainer.childCount > 0)
        {
            _overlayEffect = _overlayEffectContainer.GetChild(0).gameObject;
        }

        _shiftAreas = new List<CameraShiftArea>();
        Zoom = _camera.transform.localPosition.z;
    }

    private void FixedUpdate()
    {
        if (_target == null)
        {
            return;
        }

        var targetPosition = _target.position + _shift;
        targetPosition.x = Mathf.Max(targetPosition.x, _maxX - _margin);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _smoothTime);
        _maxX = Mathf.Max(_maxX, transform.position.x);

        _camera.transform.localPosition = Vector3.SmoothDamp(_camera.transform.localPosition, new Vector3(0, 0, _zoom),
            ref _innerVelocity, _zoomSmoothTime);
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void SwitchOverlayEffect(GameObject effectPrefab)
    {
        if (_overlayEffect != null)
        {
            Destroy(_overlayEffect.gameObject);
        }

        if (effectPrefab == null)
        {
            return;
        }

        _overlayEffect = Instantiate(effectPrefab, _overlayEffectContainer);
        _overlayEffect.transform.localPosition = Vector3.zero;
        _overlayEffect.transform.localRotation = Quaternion.identity;
        _overlayEffect.transform.localScale = Vector3.one;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var shiftArea = other.gameObject.GetComponent<CameraShiftArea>();

        if (shiftArea != null)
        {
            _shiftAreas.Add(shiftArea);
            _shift = shiftArea.Shift;

            return;
        }

        var shiftTrigger = other.gameObject.GetComponent<CameraShiftTrigger>();

        if (shiftTrigger != null)
        {
            _shift = shiftTrigger.Shift;
            shiftTrigger.gameObject.SetActive(false);

            return;
        }

        var zoomTrigger = other.gameObject.GetComponent<CameraZoomTrigger>();

        if (zoomTrigger != null)
        {
            _zoom = zoomTrigger.Zoom;
            zoomTrigger.gameObject.SetActive(false);

            return;
        }

        var zoomTimeTrigger = other.gameObject.GetComponent<CameraZoomTimeTrigger>();

        if (zoomTimeTrigger != null)
        {
            _zoomSmoothTime = zoomTimeTrigger.ZoomTime;
            zoomTimeTrigger.gameObject.SetActive(false);

            return;
        }

        var smoothTimeTrigger = other.gameObject.GetComponent<CameraSmoothTimeTrigger>();

        if (smoothTimeTrigger != null)
        {
            _smoothTime = smoothTimeTrigger.SmoothTime;
            smoothTimeTrigger.gameObject.SetActive(false);

            return;
        }

        var marginTrigger = other.gameObject.GetComponent<CameraMarginTrigger>();

        if (marginTrigger != null)
        {
            _margin = marginTrigger.Margin;
            marginTrigger.gameObject.SetActive(false);

            return;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var shiftTrigger = other.gameObject.GetComponent<CameraShiftArea>();

        if (shiftTrigger == null)
        {
            return;
        }

        if (_shiftAreas.Remove(shiftTrigger))
        {
            _shift = _shiftAreas.Count == 0 ? Vector3.zero : _shiftAreas.Last().Shift;
        }
    }
}
