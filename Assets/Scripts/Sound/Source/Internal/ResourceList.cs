using DataType;
using Sound.Structure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound.Source.Internal
{
    [System.Serializable]
    public class ResourceList : ISound
    {
        [SerializeField]
        protected ValueRange _pitch = new ValueRange(1);
        [SerializeField]
        protected ValueRange _volume = new ValueRange(1);
        [SerializeField]
        protected ValueRange _deley = new ValueRange(0);
        [SerializeField]
        protected ValueRange _loop = new ValueRange(1);
        public ValueRange Pitch { get => _pitch; set => _pitch = value; }
        public ValueRange Volume { get => _volume; set => _volume = value; }
        public ValueRange Delay { get => _deley; set => _deley = value; }
        public ValueRange Loop { get => _loop; set => _loop = value; }



        public List<ISound> Containers { get => GetContainers(); set { } }
        public virtual List<ISound> VirtualContainers => GetContainers();

        [SerializeField]
        private string _audioPool;


        [SerializeField]
        protected List<SoundBite> _soundBites = new List<SoundBite>();

        public ResourceList() { }

        public ResourceList(string pool = "") 
        {
            _audioPool = pool;
            LoadAudio(pool);
        }

        public void LoadAudio(string newAudio = null)
        {
            if (newAudio == null && _audioPool != null)
                LoadAudio(_audioPool);
            if (newAudio == _audioPool && _soundBites.Count != 0)
                return;
            if (!string.IsNullOrWhiteSpace(newAudio))
                _audioPool = newAudio;

            var clipPool = Utility.Toolbox.Instance.SoundPool.GrabBakedAudio(_audioPool);
            _soundBites.Clear();
            foreach (var clip in clipPool)
                _soundBites.Add(new SoundBite()
                {
                    Clip = clip,
                    Pitch = new ValueRange(1),
                    Volume = new ValueRange(1),
                    Delay = new ValueRange(0),
                    Loop = new ValueRange(1)
                });
        }

        public void Start() 
        {
            LoadAudio();
        }

        public ISound Clone() 
        {
            ResourceList clone = new ResourceList();
            clone.Pitch = Pitch;
            clone.Volume = Volume;
            clone.Delay = Delay;
            clone.Loop = Loop;
            clone._audioPool = _audioPool;
            clone._soundBites = new List<SoundBite>();
            foreach(SoundBite bite in _soundBites)
                clone._soundBites.Add(bite.Clone());
            return clone;
        }

        public List<SoundBite> Next() => null;
        public List<SoundBite> Update() => Next();

        private List<ISound> GetContainers() 
        {
            if (!Application.isPlaying)
                return new List<ISound>();

            List<ISound> bites = new List<ISound>();
            if (_soundBites.Count == 0)
            {
                // try Load audio clips
                LoadAudio();
                if (_soundBites.Count == 0) 
                {
                    Console.LogWarning($"Could not load {_audioPool}.");
                }
            }

            foreach (var bite in _soundBites)
            {
                
                var toReturn = new SoundBite();
                toReturn.Clip = bite.Clip;
                toReturn.Pitch = bite.Pitch * this.Pitch;
                toReturn.Volume = bite.Volume * this.Volume;
                toReturn.Delay = bite.Delay + this.Delay;
                toReturn.Loop = this.Loop;
                bites.Add(new SoundClip(toReturn));
            }
            return bites;
        }
    }
}