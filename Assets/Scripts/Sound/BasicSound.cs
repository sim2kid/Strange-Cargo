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
        private ValueRange _pitch = new ValueRange(1);
        [SerializeField]
        private ValueRange _volume = new ValueRange(1);
        [SerializeField]
        private ValueRange _deley = new ValueRange(0);
        [SerializeField]
        private bool _loop;

        [SerializeField]
        private string _audioPool;

        private List<AudioClip> _clipPool;

        public virtual ValueRange Pitch { get => _pitch; set => _pitch = value; }
        public virtual ValueRange Volume { get => _volume; set => _volume = value; }
        public virtual ValueRange Delay { get => _deley; set => _deley = value; }
        public virtual bool Loop { get => _loop; set => _loop = value; }
        public virtual AudioClip Clip { get => _clipPool[Random.Range(0, _clipPool.Count)];}


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