using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound.Structure
{
    public class SequenceContainer : Container
    {
        public override ISound Clone()
        {
            ISound clone = new SequenceContainer();
            CopyFields(this, ref clone);
            return clone;
        }

        protected override List<ISound> GetContainerInstance()
        {
            return VirtualContainers;
        }

        public override List<SoundBite> Next() 
        {
            List<SoundBite> container = base.Next();
            if (container == null)
            {
                _onContainer++;
                if (_onContainer >= _containerListLength)
                {
                    if (_onLoop < _loopDurration)
                    {
                        _onLoop++;
                        _onContainer = 0;
                        return Next();
                    }
                    return null;
                }
                else 
                {
                    return Next();
                }
            }
            return container;
        }
    }
}