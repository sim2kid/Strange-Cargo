using DataType;
using Sound.Structure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound.Source.Internal
{
    [System.Serializable]
    public class ResourceList : BasicSound, ISound
    {
        [Header("Resource List")]
        [SerializeField]
        private string _audioPool;

        [SerializeField]
        protected List<SoundBite> _soundBites = new List<SoundBite>();
        public override List<SoundBite> Bites => GetBites();

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
                    Delay = new ValueRange(0)
                });
        }

        private List<SoundBite> GetBites() 
        {
            List<SoundBite> bites = new List<SoundBite>();
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
                bites.Add(toReturn);
            }
            return bites;
        }
    }
}