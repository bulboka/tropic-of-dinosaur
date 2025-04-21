using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Sound
{
    public enum RandomizedSoundMode {Random, Sequence}

    [RequireComponent(typeof(AudioSource))]
    public class RandomizedAudioSource : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        [SerializeField] private List<AudioClip> _audioClips;
        
        [SerializeField] private float _volumeMin = 0.8f;

        [SerializeField] private float _volumeMax = 1f;

        [SerializeField] private float _pitchMin = 0.8f;

        [SerializeField] private float _pitchMax = 1.2f;
        
        [SerializeField] private RandomizedSoundMode _mode;
        [SerializeField] private float _panMin = 0;
        [SerializeField] private float _panMax = 0;

        private int _currentSoundIndex;
        private AudioSource _currentAudioSource;

        public void PlaySound()
        {
            PlaySound(0, true);
        }
        public void PlaySound(float delay = 0, bool isOneShot = false)
        {
            PlaySound(delay, 1f, null, isOneShot);
        }
        
        public void PlaySound(float delay, AudioSource externalAudioSource)
        {
            PlaySound(delay, 1f, externalAudioSource);
        }
        
        public void PlaySound(float delay, float volumeMult, AudioSource externalAudioSource = null, bool isOneShot = false)
        {
            _currentAudioSource = externalAudioSource == null ? _audioSource : externalAudioSource;
            
            _currentAudioSource.clip = _mode switch
            {
                RandomizedSoundMode.Random => _audioClips[Random.Range(0, _audioClips.Count)],
                RandomizedSoundMode.Sequence => _audioClips[_currentSoundIndex++],
                _ => _currentAudioSource.clip
            };

            if (_currentSoundIndex >= _audioClips.Count)
            {
                _currentSoundIndex = 0;
            }
            
            _currentAudioSource.volume = Random.Range(_volumeMin, _volumeMax) * volumeMult;
            _currentAudioSource.pitch = Random.Range(_pitchMin, _pitchMax);
            _currentAudioSource.panStereo = Random.Range(_panMin, _panMax);


            if (delay > 0)
            {
                _currentAudioSource.PlayDelayed(delay);
            }
            else if (isOneShot)
            {
                _currentAudioSource.PlayOneShot(_currentAudioSource.clip);
            }
            else
            {
                _currentAudioSource.Play();
            }
        }

        public void StopSound()
        {
            _currentAudioSource.DOFade(0, 0.2f);
        }

        public void Dispose()
        {
            DOTween.Kill(_audioSource);
        }

        public void SetAudioClips(List<AudioClip> clips)
        {
            _audioClips = clips;
        }
    }
}