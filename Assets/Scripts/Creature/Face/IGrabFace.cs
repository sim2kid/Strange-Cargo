using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Face
{
    public interface IGrabFace
    {
        /// <summary>
        /// Populate the Face Grabber with creature info
        /// </summary>
        /// <param name="creatureDna">The DNA of the creature whos face you want to grab.</param>
        public void Hydrate(Genetics.DNA creatureDna);

        /// <summary>
        /// Grabs the eyes for a creature.
        /// </summary>
        /// <param name="action">The name/action of the eyes that you're grabbing. Eg: 'blink'</param>
        /// <returns>Loaded Texture of Eyes</returns>
        public Texture2D GrabEyes(string action);

        /// <summary>
        /// Grabs the mouth for a creature.
        /// </summary>
        /// <param name="action">The name/action of the mouth that you're grabbing. Eg: 'chewing'</param>
        /// <returns>Loaded Texture of Mouth</returns>
        public Texture2D GrabMouth(string action);
    }
}