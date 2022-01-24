using DataType;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound.Structure
{
    [System.Serializable]
    public class RandomContainer : Container
    {
        protected override List<ISound> GetContainerInstance() 
        {
            List<ISound> containers = new List<ISound>();
            var virCon = VirtualContainers;
            if (virCon.Count > 0) 
            {
                ISound sound = virCon[Random.Range(0, VirtualContainers.Count)];
                if (sound != null)
                {
                    sound = ApplyProperties(sound.Clone());
                    containers.Add(sound);
                }
            }
            return containers;
        }

        public override ISound Clone()
        {
            ISound clone = new RandomContainer();
            CopyFields(this, ref clone);
            return clone;
        }
    }
}