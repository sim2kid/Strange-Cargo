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
        protected SoundBite _bite;

        public ValueRange Pitch { get => _bite.Pitch; set => _bite.Pitch = value; }
        public ValueRange Volume { get => _bite.Volume; set => _bite.Volume = value; }
        public ValueRange Delay { get => _bite.Delay; set => _bite.Delay = value; }

        public List<SoundBite> Bites => new List<SoundBite>() { _bite };

        public List<ISound> Containers { get => null; set { } }
    }
}