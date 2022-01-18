using DataType;
using Sound.Structure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound.Source.Internal
{
    [System.Serializable]
    public class ResourceClip : ISound
    {
        [SerializeField]
        private string _audioFile;

        [SerializeField]
        protected SoundBite _soundBite;

        public ValueRange Pitch { get => _soundBite.Pitch; set => _soundBite.Pitch = value; }
        public ValueRange Volume { get => _soundBite.Volume; set => _soundBite.Volume = value; }
        public ValueRange Delay { get => _soundBite.Delay; set => _soundBite.Delay = value; }

        public List<SoundBite> Bites => GetBites();

        public List<ISound> Containers { get => null; set { } }
        public List<ISound> VirtualContainers => null;

        public ResourceClip() { }
        public ResourceClip(string audioFile = null) 
        {
            _audioFile = audioFile;
            LoadAudio(audioFile);
        }

        public void LoadAudio(string newAudio = null)
        {
            if (newAudio == null && _audioFile != null)
                LoadAudio(_audioFile);
            if (newAudio == _audioFile && _soundBite.Clip != null)
                return;
            if (!string.IsNullOrWhiteSpace(newAudio))
                _audioFile = newAudio;

            var clip = Utility.Toolbox.Instance.SoundPool.GrabSingleBakedAudio(_audioFile);
            _soundBite = new SoundBite()
            {
                Clip = clip,
                Pitch = new ValueRange(1),
                Volume = new ValueRange(1),
                Delay = new ValueRange(0)
            };
        }

        private List<SoundBite> GetBites()
        {
            List<SoundBite> bites = new List<SoundBite>();
            if (_soundBite.Clip == null)
            {
                // try Load audio clips
                LoadAudio();
                if (_soundBite.Clip == null)
                {
                    Console.LogWarning($"Could not load {_audioFile}.");
                }
            }

            bites.Add(_soundBite);

            return bites;
        }
    }
}
