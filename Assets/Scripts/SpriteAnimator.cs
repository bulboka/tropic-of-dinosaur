using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private List<Sprite> _sprites;
    [SerializeField] private float _frameDuration;

    private float _nextFrameTime;
    private int _currentSpriteIndex;

    private void Update()
    {
        if (Time.time < _nextFrameTime)
        {
            return;
        }

        _nextFrameTime = Time.time + _frameDuration;

        var newSpriteIndex = _currentSpriteIndex;

        while (newSpriteIndex == _currentSpriteIndex)
        {
            newSpriteIndex = Random.Range(0, _sprites.Count);
        }

        _currentSpriteIndex = newSpriteIndex;
        _spriteRenderer.sprite = _sprites[_currentSpriteIndex];
    }
}