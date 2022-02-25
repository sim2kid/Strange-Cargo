using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Creature.Face
{
    public class DemoGrabFace : IGrabFace
    {
        public void Hydrate(Genetics.DNA creatureDna) 
        {
            // Do Work Here
        }

        public Texture2D GrabEyes(string action) 
        {
            // Return default eyes
            return null;
        }
        public Texture2D GrabMouth(string action) 
        {
            // Return default mouth
            return null;
        }
    }
}