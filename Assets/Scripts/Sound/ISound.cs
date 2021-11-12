using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataType;

namespace Sound
{
    public interface ISound 
    {
        /// <summary>
        /// The relative pitch of the audio clip
        /// </summary>
        public ValueRange Pitch { get; set; }
        /// <summary>
        /// The relative volume of the audio clip
        /// </summary>
        public ValueRange Volume { get; set; }
        /// <summary>
        /// The relative delay before the start of an audio clip
        /// </summary>
        public ValueRange Delay { get; set; }
        /// <summary>
        /// Whether to loop a clip or not
        /// </summary>
        public bool Loop { get; set; }

        /// <summary>
        /// The audio clip from a given sound
        /// </summary>
        public AudioClip Clip { get; }
    }
}