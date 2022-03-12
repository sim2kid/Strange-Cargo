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

        public SilentSource() { }
        public void Start() { }
        public void Prime()
        {
            Start();
        }
        public ISound Clone() 
        {
            SilentSource clone = new SilentSource();
            clone._deley = _deley;
            clone._loop = _loop;
            return clone;
        }
        public List<SoundBite> Next() => new List<SoundBite>();
        public List<SoundBite> Update() => Next();
    }
}