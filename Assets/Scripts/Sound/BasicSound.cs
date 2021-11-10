using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataType;

namespace Sound
{
    [System.Serializable]
    public class BasicSound : ISound
    {
        [SerializeField]
        protected ValueRange _pitch = new ValueRange(1);
        [SerializeField]
        protected ValueRange _volume = new ValueRange(1);
        [SerializeField]
        protected ValueRange _deley = new ValueRange(0);
        [SerializeField]
        protected bool _loop;

        [SerializeField]
        private string _audioPool;

        protected List<AudioClip> _clipPool;

        public virtual ValueRange Pitch { get => _pitch; set => _pitch = value; }
        public virtual ValueRange Volume { get => _volume; set => _volume = value; }
        public virtual ValueRange Delay { get => _deley; set => _deley = value; }
        public virtual bool Loop { get => _loop; set => _loop = value; }
        public virtual AudioClip Clip { get => GetClip(); }

        private AudioClip GetClip() 
        {
            if (_clipPool == null) 
            {
                LoadAudio();
                if (_clipPool == null) 
                {
                    Debug.LogWarning($"Could not load {_audioPool}.");
                    return null;
                }
                if (_clipPool.Count == 0)
                {
                    Debug.LogWarning($"Could not load {_audioPool}.");
                    return null;
                }
            }
            return _clipPool[Random.Range(0, _clipPool.Count)];
        }

        public void LoadAudio(string newAudio = null) 
        {
            if (newAudio == _audioPool && _clipPool == null)
                return;
            if (!string.IsNullOrWhiteSpace(newAudio))
                _audioPool = newAudio;
            
            _clipPool = Utility.Toolbox.Instance.SoundPool.GrabBakedAudio(_audioPool);
        }
    }
}