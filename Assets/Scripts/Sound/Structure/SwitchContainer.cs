using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound.Structure
{
    [System.Serializable]
    public class SwitchContainer : Container
    {
        private int _sel = 0;
        public int Selection { get => _sel; set { _sel = Mathf.Clamp(value, 0, VirtualContainers.Count-1); } }

        protected override List<SoundBite> GetBites()
        {
            List<SoundBite> soundBites = new List<SoundBite>();
            var virCon = VirtualContainers;
            if (virCon.Count > 0)
            {
                Selection = Mathf.Clamp(Selection, 0, virCon.Count - 1);
                ISound sound = virCon[Selection];
                if (sound != null)
                    foreach (var bite in sound.Bites)
                    {
                        soundBites.Add(CopyBite(bite));
                    }
            }
            return soundBites;
        }
    }
}