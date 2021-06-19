using PigeonProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PigeonProject
{
    [CreateAssetMenu(menuName = "PigeonProject/Audio/SoundCollection")]
    public class SoundCollection : AudioPlayable
    {
        #region Serialized Fields
        [SerializeField] private Sound[] _sounds = default;
        #endregion

        #region Fields
        private AudioSource _currentAudioSource = default;
        #endregion


        #region Override methods
        public override AudioSource Play(GameObject source)
        { 
            _currentAudioSource = GetRandomSound().Play(source);
            return _currentAudioSource;
        }

        public override void Play(AudioSource source)
        {
            GetRandomSound().Play(source);
            _currentAudioSource = source;
        }

        public override void Stop(GameObject source)
        {
            if (_currentAudioSource != null)
                _currentAudioSource?.Stop();
        }
        #endregion

        #region Utility
        private Sound GetRandomSound()
        {
            int randomIndex = Random.Range(0, _sounds.Length - 1);
            return _sounds[randomIndex];
        }
        #endregion
    }
}
