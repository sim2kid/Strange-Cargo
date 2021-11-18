using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Environment;

namespace Creature.Brain
{
    [System.Serializable]
    public class Preferred
    {
        public IObject Obj;
        public float Preference;

        public Preferred(IObject preferredObj) : this(preferredObj, 0) { }
        public Preferred(IObject preferredObj, float preference)
        {
            Obj = preferredObj;
            this.Preference = preference;
        }
    }
}