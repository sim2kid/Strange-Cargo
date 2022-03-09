using Genetics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Face
{
    public class GrabFace : IGrabFace
    {
        string FaceType;

        public GrabFace()
        {
            FaceType = "Generic";
        }

        public Texture2D GrabEyes(string action)
        {
            throw new System.NotImplementedException();
        }

        public Texture2D GrabMouth(string action)
        {
            throw new System.NotImplementedException();
        }

        public void Hydrate(DNA creatureDna)
        {
            if (!string.IsNullOrEmpty(creatureDna.FaceType))
            {
                this.FaceType = creatureDna.FaceType;
            }
            else 
            {
                Console.LogWarning($"Creature with no face type has been detected.");
            }
        }
    }
}