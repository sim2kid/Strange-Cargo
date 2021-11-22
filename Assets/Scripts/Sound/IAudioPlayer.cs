using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound
{
    public interface IAudioPlayer
    {
        /// <summary>
        /// True When an Audio is Playing (Read Only)
        /// </summary>
        public bool IsPlaying { get; }
        /// <summary>
        /// True when an audio will play but is waiting for a delay first (Read Only)
        /// </summary>
        public bool IsDelayed { get; }

        /// <summary>
        /// The current volume of the clip being played
        /// </summary>
        public float Volume { get; set; }

        /// <summary>
        /// Plays the sound
        /// </summary>
        public void Play();
        /// <summary>
        /// Stops the sound
        /// </summary>
        public void Stop();
        /// <summary>
        /// Pauses the sound
        /// </summary>
        public void Pause();
        /// <summary>
        /// Unpauses the sound
        /// </summary>
        public void UnPause();
    }
}