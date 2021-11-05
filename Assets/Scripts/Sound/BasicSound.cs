using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sound
{
    [Serializable]
    public class BasicSound : ISound
    {
        [SerializeField]
        private float _pitch;
        [SerializeField]
        private float _volume;
        [SerializeField]
        private float _deley;
        [SerializeField]
        private bool _loop;

        [SerializeField]
        private AudioClip _clip;

        public virtual float Pitch { get => _pitch; set => _pitch = value; }
        public virtual float Volume { get => _volume; set => _volume = value; }
        public virtual float Delay { get => _deley; set => _deley = value; }
        public virtual bool Loop { get => _loop; set => _loop = value; }
        public virtual AudioClip Clip { get => _clip; set => _clip = value; }
    }
}