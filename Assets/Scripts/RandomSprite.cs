using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private List<Sprite> _sprites;

    private void Start()
    {
        if (_sprites.Count > 0)
        {
            _spriteRenderer.sprite = _sprites[Random.Range(0, _sprites.Count)];
        }
    }
}