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

        protected override List<ISound> GetContainerInstance()
        {
            List<ISound> containers = new List<ISound>();
            var virCon = VirtualContainers;
            if (virCon.Count > 0)
            {
                Selection = Mathf.Clamp(Selection, 0, virCon.Count - 1);
                ISound sound = virCon[Selection];
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
            ISound clone = new SwitchContainer();
            CopyFields(this, ref clone);
            SwitchContainer cont = (SwitchContainer)clone;
            cont.Selection = this.Selection;
            return cont;
        }
    }
}