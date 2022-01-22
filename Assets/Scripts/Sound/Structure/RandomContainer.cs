using DataType;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound.Structure
{
    [System.Serializable]
    public class RandomContainer : Container
    {
        protected override List<SoundBite> GetBites()
        {
            List<SoundBite> soundBites = new List<SoundBite>();
            var virCon = VirtualContainers;
            if (virCon.Count > 0)
            {
                ISound sound = virCon[Random.Range(0, VirtualContainers.Count)];
                if(sound != null)
                    foreach (var bite in sound.Bites)
                    {
                        soundBites.Add(CopyBite(bite));
                    }
            }
            return soundBites;
        }

    }
}