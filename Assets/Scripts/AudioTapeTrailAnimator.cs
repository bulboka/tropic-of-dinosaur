using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioTapeTrailAnimator : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private List<Texture> _textures;
    [SerializeField] private float _frameDuration;

    private float _nextFrameTime;
    private Texture _initialTexture;

    private void Start()
    {
        _initialTexture = _material.mainTexture;
    }

    private void OnDestroy()
    {
        _material.mainTexture = _initialTexture;
    }

    private void Update()
    {
        if (Time.time < _nextFrameTime)
        {
            return;
        }

        _nextFrameTime = Time.time + _frameDuration;
        _material.mainTexture = _textures[Random.Range(0, _textures.Count)];
    }
}