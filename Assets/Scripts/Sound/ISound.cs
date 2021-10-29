using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound
{
    public interface ISound 
    {
        /// <summary>
        /// The relitive pitch of the audio clip
        /// </summary>
        public float Pitch { get; set; }
        /// <summary>
        /// The relitive volume of the audio clip
        /// </summary>
        public float Volume { get; set; }
        /// <summary>
        /// The relitive delay before the start of an audio clip
        /// </summary>
        public float Delay { get; set; }
        /// <summary>
        /// Whether to loop a clip or not
        /// </summary>
        public bool Loop { get; set; }

        /// <summary>
        /// True When an Audio is Playing (Read Only)
        /// </summary>
        public bool IsPlaying { get; }
        /// <summary>
        /// True when an audio will play but is waiting for a delay first (Read Only)
        /// </summary>
        public bool IsDelayed { get; }
        /// <summary>
        /// The sound that will be played next. (Read Only)
        /// </summary>
        public ISound Sound { get; }

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