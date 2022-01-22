using Sound.Structure;
using DataType;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound.Source
{
    [System.Serializable]
    public class SilentSource : ISound
    {
        [SerializeField]
        protected ValueRange _deley = new ValueRange(0);
        [SerializeField]
        protected ValueRange _loop = new ValueRange(1);
        public ValueRange Pitch { get => new ValueRange(1); set { } }
        public ValueRange Volume { get => new ValueRange(1); set { } }
        public ValueRange Delay { get => _deley; set => _deley = value; }
        public ValueRange Loop { get => _loop; set => _loop = value; }



        public List<ISound> Containers { get => null; set { } }
        public virtual List<ISound> VirtualContainers => null;
        public List<SoundBite> Bites => new List<SoundBite>();

        public SilentSource() { }
    }
}