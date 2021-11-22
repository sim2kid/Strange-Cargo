﻿using DataType;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sound
{
    [System.Serializable]
    public class ExternalSound : MonoBehaviour, ISound
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
        string _audioPool;

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
                    Console.LogWarning($"Could not load {_audioPool}.");
                    return null;
                }
                if (_clipPool.Count <= 0)
                {
                    Console.LogWarning($"Could not load {_audioPool}.");
                    return null;
                }
            }
            if (_clipPool.Count > 0)
                return _clipPool[Random.Range(0, _clipPool.Count)];
            else
                return null;
        }

        public void LoadAudio(string newAudio = null)
        {
            if (newAudio == _audioPool && _clipPool == null)
                return;
            if (!string.IsNullOrWhiteSpace(newAudio))
                _audioPool = newAudio;

            StartCoroutine(Utility.Toolbox.Instance.SoundPool.GrabLiveAudio(_audioPool, (List<AudioClip> clips) =>
            {
                _clipPool = clips;
            }));
        }
    }
}
