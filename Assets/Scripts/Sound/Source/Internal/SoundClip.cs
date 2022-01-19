using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sound.Structure;
using DataType;

namespace Sound.Source.Internal
{
    [System.Serializable]
    public class SoundClip : ISound
    {
        [SerializeField]
        protected SoundBite _soundBite;

        public ValueRange Pitch { get => _soundBite.Pitch; set => _soundBite.Pitch = value; }
        public ValueRange Volume { get => _soundBite.Volume; set => _soundBite.Volume = value; }
        public ValueRange Delay { get => _soundBite.Delay; set => _soundBite.Delay = value; }
        public ValueRange Loop { get => _soundBite.Loop; set => _soundBite.Loop = value; }


        public List<SoundBite> Bites => new List<SoundBite>() { _soundBite };

        public List<ISound> Containers { get => null; set { } }
        public List<ISound> VirtualContainers => null;

        public SoundClip() 
        {
        
        }
        public SoundClip(SoundBite bite) 
        {
            _soundBite = bite;
        }
    }
}