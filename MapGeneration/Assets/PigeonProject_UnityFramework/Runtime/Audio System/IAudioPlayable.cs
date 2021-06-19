using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PigeonProject
{
    interface IAudioPlayable
    {
        /// <summary>
        /// Plays the AudioClip using the settings from the AusioPlayable instance.
        /// </summary>
        /// <remarks>
        /// Attaches a AudioSource to the source GameObject.
        /// </remarks>
        /// <param name="source">The source GameObject that plays the sound.</param>
        AudioSource Play(GameObject source);
        /// <summary>
        /// Plays the AudioClip using the settings from the AudioSource.
        /// </summary>
        /// <param name="source">The AudioSource that plays the sound.</param>
        void Play(AudioSource source);

        void Stop(GameObject source);
    }
}
