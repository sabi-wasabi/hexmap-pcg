using PigeonProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PigeonProject
{
    [CreateAssetMenu(menuName = "PigeonProject/Audio/Sound")]
    public class Sound : AudioPlayable
    {
        #region Serialized Fields
        [Header("Clip")]
        [SerializeField] private AudioClip _audioClip = default;
        [Header("Settings")]
        [Tooltip("Enable this to make the Audio Clip loop when it reaches the end.")]
        [SerializeField] bool _loop = false;
        [Tooltip("How loud the sound is at a distance of one world unit (one meter) from the Audio Listener.")]
        [SerializeField] [Range(0f, 1f)] private float _volume = 1f;
        [Space()]
        [Tooltip("Amount of change in pitch due to slowdown/speed up of the Audio Clip. Value 1 is normal playback speed.")]
        [SerializeField] [Range(-3f, 3f)] private float _pitch = 1f;
        [SerializeField] bool _randomisePitch = false;
        [SerializeField] [Range(-3f, 3f)] private float _minPitch = 1f;
        [SerializeField] [Range(-3f, 3f)] private float _maxPitch = 1f;
        [Space()]
        [Tooltip("Sets the position in the stereo field of 2D sounds. -1 is Left; 1 is Right.")]
        [SerializeField] [Range(-1f, 1f)] private float _stereoPan = 0f;
        [Space()]
        [Tooltip("Sets how much the 3D engine has an effect on the audio source.")]
        [SerializeField] [Range(0, 1f)] private float _spatialBlend = 0f;
        #endregion

        #region Fields
        private readonly IDictionary<GameObject, AudioSource> _sources = new Dictionary<GameObject, AudioSource>();
        #endregion

        private void OnDisable()
        {
            foreach (var element in _sources)
            {
                Destroy(element.Value);
            }
            _sources.Clear();
        }

        #region Override methods
        public override AudioSource Play(GameObject sourceObject)
        {
            AudioSource audioSource = GetAudioSource(sourceObject);
            audioSource.Play();
            return audioSource;
        }

        public override void Play(AudioSource source)
        {
            source.clip = _audioClip;
            source.Play();
        }

        public override void Stop(GameObject sourceObject)
        {
            if (_sources.ContainsKey(sourceObject))
            {
                _sources[sourceObject].Stop();
            }
        }
        #endregion

        #region Utility
        private void SetupAudioSource(AudioSource audioSource)
        {
            audioSource.volume = _volume;
            audioSource.clip = _audioClip;
            audioSource.playOnAwake = false;
            audioSource.loop = _loop;
            audioSource.pitch = GetPitch();
            audioSource.spatialBlend = _spatialBlend;
            audioSource.panStereo = _stereoPan;
        }

        private float GetPitch()
        {
            if (!_randomisePitch)
                return _pitch;
            float min = Mathf.Min(_minPitch, _maxPitch);
            float max = Mathf.Max(_minPitch, _maxPitch);
            return Random.Range(min, max);
        }

        private AudioSource GetAudioSource(GameObject sourceObject)
        {
            if (!_sources.TryGetValue(sourceObject, out AudioSource audioSource))
            {
                audioSource = sourceObject.AddComponent<AudioSource>();
                SetupAudioSource(audioSource);
                _sources.Add(sourceObject, audioSource);
            }

            return audioSource;
        }
        #endregion
    }
}
