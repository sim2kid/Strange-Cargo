using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Environment;
using Newtonsoft.Json;

namespace Creature.Brain
{
    [System.Serializable]
    public class Preferred
    {
        [JsonConverter(typeof(PersistentData.Loading.GenericObject))]
        public IObject Obj;
        public float Preference;

        public Preferred(IObject preferredObj) : this(preferredObj, 0) { }
        public Preferred(IObject preferredObj, float preference)
        {
            Obj = new UnknownObject(preferredObj);
            this.Preference = preference;
        }
        public Preferred() : this(new UnknownObject()) { }
    }
}