using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PigeonProject
{
    public abstract class AudioPlayable : ScriptableObject, IAudioPlayable
    {
        public abstract AudioSource Play(GameObject source);
        public abstract void Play(AudioSource source);
        public abstract void Stop(GameObject source);
    }
}
