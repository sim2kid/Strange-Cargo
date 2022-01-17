using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataType;

namespace Sound.Structure
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
        /// The sound bites that compose a single sound to be played
        /// </summary>
        public List<SoundBite> Bites { get; }

        /// <summary>
        /// The list of containers that make up a sound.
        /// Null if it can't hold containers.
        /// </summary>
        public List<ISound> VirtualContainers { get; set; }

        /// <summary>
        /// The list of containers that effectivly make up a sound.
        /// Null if it can't hold containers.
        /// </summary>
        public List<ISound> Containers { get; }
    }
}