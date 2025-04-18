using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ColorAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private List<Color> _colors;
    [SerializeField] private float _frameDuration;

    private float _nextFrameTime;
    private int _currentColorIndex;

    private void Update()
    {
        if (Time.time < _nextFrameTime)
        {
            return;
        }

        _nextFrameTime = Time.time + _frameDuration;

        var currentColorIndex = _currentColorIndex;

        while (currentColorIndex == _currentColorIndex)
        {
            currentColorIndex = Random.Range(0, _colors.Count);
        }

        _currentColorIndex = currentColorIndex;
        _spriteRenderer.color = _colors[_currentColorIndex];
    }
}