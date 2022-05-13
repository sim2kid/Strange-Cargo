using DataType;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound.Structure
{
    [System.Serializable]
    public class SoundBite
    {
        public AudioClip Clip;
        public ValueRange Pitch = 1;
        public ValueRange Volume = 1;
        public ValueRange Delay = 0;
        public ValueRange Loop = 1;
        [HideInInspector]
        public int loopCount = -1;

        public SoundBite Clone() 
        {
            SoundBite clone = new SoundBite();
            clone.Clip = Clip;
            clone.Pitch = Pitch;
            clone.Volume = Volume;
            clone.Delay = Delay;
            clone.Loop = Loop;
            return clone;
        }
    }
}