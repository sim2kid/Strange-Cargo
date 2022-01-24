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
        /// The number of times an audio should play. -1 is indefinite
        /// </summary>
        public ValueRange Loop { get; set; }

        /// <summary>
        /// The list of all real containers that make up a sound.
        /// Null if it can't hold containers.
        /// </summary>
        public List<ISound> Containers { get; set; }

        /// <summary>
        /// The list of containers that effectivly make up a sound.
        /// This list is processed based on the Containers list
        /// Null if it can't hold containers.
        /// </summary>
        public List<ISound> VirtualContainers { get; }

        /// <summary>
        /// Sets up the container to be itterated over using the <seealso cref="Next"/> method.
        /// </summary>
        public void Start();

        /// <summary>
        /// Processes and gets the next sound bites to be played in this container. 
        /// Please use <see cref="Start"/> to initialize the container.
        /// </summary>
        /// <returns>Returns SoundBite to play and Null when finished. Source will never null.</returns>
        public List<SoundBite> Next();

        /// <summary>
        /// Make a copy of an ISound with different instances of each object
        /// </summary>
        /// <returns>A copy of the original ISound</returns>
        public ISound Clone();
    }
}