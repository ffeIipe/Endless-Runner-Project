using System;
using Enums;
using Scriptables;
using UnityEngine;
using UnityEngine.Audio;

namespace Managers.SoundManagerFolder
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private SoundManagerData soundManagerData; 
        public static SoundManager Instance { get; private set; }
        private AudioSource _audioSource;

        private float _globalVolume;
        
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            EventManager.UIEvents.OnSoundVolumeChanged += OnSoundVolumeChanged;
        }

        private void OnDisable()
        {
            EventManager.UIEvents.OnSoundVolumeChanged -= OnSoundVolumeChanged;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void PlaySound(SoundType sound, AudioSource source = null, float volume = 1, float  pitchMin = 1f, float  pitchMax = 1f)
        {
            if (soundManagerData.Sounds[(int)sound].sounds.Length <= 0)
            {
                Debug.LogWarning("No sounds with sound: " + sound);
                return;
            }
            
            var soundList = soundManagerData.Sounds[(int)sound];
            var clips = soundList.sounds;
            var randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];

            var pitch = 1f;
            if (!Mathf.Approximately(pitchMax, 1) && !Mathf.Approximately(pitchMin, 1))
            {
                pitch = UnityEngine.Random.Range(pitchMin, pitchMax);
            }
            
            if (source)
            {
                source.outputAudioMixerGroup = soundList.mixer;
                source.clip = randomClip;
                //source.volume = volume * soundList.volume;
                source.volume = _globalVolume;
                source.pitch = pitch;
                source.PlayOneShot(randomClip);
            }
            else
            {
                _audioSource.outputAudioMixerGroup = soundList.mixer;
                _audioSource.PlayOneShot(randomClip, volume * soundList.volume);
            }
        }
        
        private void OnSoundVolumeChanged(float newVolume)
        {
            Debug.Log($"Sound volume changed to {newVolume}");
            _globalVolume = newVolume;
        }
    }

    [Serializable]
    public struct SoundList
    {
        [HideInInspector] public string name;
        [Range(0, 1)] public float volume;
        public AudioMixerGroup mixer;
        public AudioClip[] sounds;
    }
}
