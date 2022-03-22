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

        protected int _onLoop = 0;
        protected int _loopDurration = 0;
        protected int _onContainer = 0;
        protected int _lastContainer = 0;
        protected int _containerListLength = 0;
        protected List<ISound> _containerInstance = null;

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
                container.Start();
                if (container.Next() == null && container.VirtualContainers != null)
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

        protected abstract List<ISound> GetContainerInstance();

        public void Prime() 
        {
            foreach (var container in _containers) 
            {
                container.Prime();
            }
        }

        public virtual void Start()
        {
            _onLoop = 1;
            _loopDurration = (int)Mathf.Round(Loop.Read());
            _onContainer = 0;
            _lastContainer = -1;
            _containerInstance = GetContainerInstance();
            _containerListLength = _containerInstance.Count;
        }

        public virtual List<SoundBite> Next()
        {
            if (_onContainer >= _containerListLength)
            {
                // if can loop                or infa loops
                if (_onLoop < _loopDurration || _loopDurration == -1)
                {
                    _onLoop++;
                    _onContainer = 0;
                    return Next();
                }
                return null;
            }
            var container = _containerInstance[_onContainer];
            if (_lastContainer != _onContainer)
            {
                container.Start();
                _lastContainer = _onContainer;
            }
            if (container == null)
            {
                _onContainer++;
                return Next();
            }
            if (!(container is Container)) // if not Container but Source
            {
                // will always return a value, so we increment to the next container to prevent a loop
                _onContainer++;
            }
            return container.Next();
        }

        public virtual List<SoundBite> Update() 
        {
            var container = _containerInstance[_onContainer];
            return container?.Update();
        }

        public abstract ISound Clone();

        protected static void CopyFields(ISound original, ref ISound clone) 
        {
            clone.Pitch = original.Pitch;
            clone.Volume = original.Volume;
            clone.Delay = original.Delay;
            clone.Loop = original.Loop;
            clone.Containers = new List<ISound>();
            foreach (ISound container in original.Containers) 
            {
                clone.Containers.Add(container.Clone());
            }
        }

        protected ISound ApplyProperties(ISound origninal) 
        {
            origninal.Pitch *= this.Pitch;
            origninal.Volume *= this.Volume;
            origninal.Delay += this.Delay;
            return origninal;
        }
    }
}