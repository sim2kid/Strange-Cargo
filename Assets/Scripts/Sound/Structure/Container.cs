using DataType;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound.Structure
{
    public abstract class Container : ISound
    {
        [SerializeField]
        protected ValueRange _pitch = new ValueRange(1);
        [SerializeField]
        protected ValueRange _volume = new ValueRange(1);
        [SerializeField]
        protected ValueRange _deley = new ValueRange(0);
        [SerializeField]
        protected ValueRange _loop = new ValueRange(1);
        public virtual ValueRange Pitch { get => _pitch; set => _pitch = value; }
        public virtual ValueRange Volume { get => _volume; set => _volume = value; }
        public virtual ValueRange Delay { get => _deley; set => _deley = value; }
        public virtual ValueRange Loop { get => _loop; set => _loop = value; }

        public virtual List<SoundBite> Bites => GetBites();

        [SerializeReference]
        [SerializeField]
        protected List<ISound> _containers = new List<ISound>();
        public virtual List<ISound> Containers { get => _containers; set { _containers = value; } }
        public virtual List<ISound> VirtualContainers => ProcessContainers();

        protected virtual List<ISound> ProcessContainers() 
        {
            List<ISound> sounds = new List<ISound>();
            foreach (var container in _containers)
            {
                if (container == null)
                    continue;
                if (container.Bites == null && container.VirtualContainers != null)
                {
                    sounds.AddRange(container.VirtualContainers);
                }
                else 
                {
                    sounds.Add(container);
                }
            }
            return sounds;
        }

        protected abstract List<SoundBite> GetBites();

        protected virtual SoundBite CopyBite(SoundBite bite) 
        {
            SoundBite toAdd = new SoundBite();
            toAdd.Clip = bite.Clip;
            toAdd.Pitch = bite.Pitch * this.Pitch;
            toAdd.Volume = bite.Volume * this.Volume;
            toAdd.Delay = bite.Delay + this.Delay;
            toAdd.Loop = bite.Loop + this.Loop;
            return toAdd;
        }
    }
}