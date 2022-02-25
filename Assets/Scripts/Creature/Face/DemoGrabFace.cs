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
            return Resources.Load<Texture2D>("Face/Generic/Eyes/Neutral");
        }
        public Texture2D GrabMouth(string action) 
        {
            // Return default mouth
            return Resources.Load<Texture2D>("Face/Generic/Mouth/Neutral_Happy");
        }
    }
}