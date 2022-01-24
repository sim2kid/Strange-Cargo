using DataType;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound.Structure
{
    [System.Serializable]
    public struct SoundBite
    {
        public AudioClip Clip;
        public ValueRange Pitch;
        public ValueRange Volume;
        public ValueRange Delay;
        public ValueRange Loop;

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