using UnityEngine;

public class BlueHand : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;
    [SerializeField] private Color _randomColor1;
    [SerializeField] private Color _randomColor2;
    [SerializeField] private float _minAnimationSpeed;
    [SerializeField] private float _maxAnimationSpeed;
    [SerializeField] private float _minAnimationShift;
    [SerializeField] private float _maxAnimationShift;

    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Shift = Animator.StringToHash("Shift");

    private void Start()
    {
        _spriteRenderer.color = Color.Lerp(_randomColor1, _randomColor2, Random.value);
        _animator.SetFloat(Speed, Random.Range(_minAnimationSpeed, _maxAnimationSpeed));
        _animator.SetFloat(Shift, Random.Range(_minAnimationShift, _maxAnimationShift));
    }
}
