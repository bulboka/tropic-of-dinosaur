using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _smoothTime;
    [SerializeField] private float _margin;
    [SerializeField] private Transform _overlayEffectContainer;

    private Vector3 _velocity;
    private float _maxX;
    private GameObject _overlayEffect;

    private void Start()
    {
        _maxX = transform.position.x;

        if (_overlayEffectContainer.childCount > 0)
        {
            _overlayEffect = _overlayEffectContainer.GetChild(0).gameObject;
        }
    }

    private void LateUpdate()
    {
        if (_target == null)
        {
            return;
        }

        var targetPosition = _target.position;
        targetPosition.x = Mathf.Max(targetPosition.x, _maxX - _margin);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _smoothTime);
        _maxX = Mathf.Max(_maxX, transform.position.x);
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

        _overlayEffect = Instantiate(effectPrefab, _overlayEffectContainer);
        _overlayEffect.transform.localPosition = Vector3.zero;
        _overlayEffect.transform.localRotation = Quaternion.identity;
        _overlayEffect.transform.localScale = Vector3.one;
    }
}
