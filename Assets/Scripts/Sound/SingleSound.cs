using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sound
{
    [Serializable]
    public class SingleSound : ISound
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

        public float Pitch { get => _pitch; set => _pitch = value; }
        public float Volume { get => _volume; set => _volume = value; }
        public float Delay { get => _deley; set => _deley = value; }
        public bool Loop { get => _loop; set => _loop = value; }
        public AudioClip Clip { get => _clip; set => _clip = value; }
    }
}