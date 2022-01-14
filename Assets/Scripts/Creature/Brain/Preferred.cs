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
        [JsonIgnore]
        IObject obj;
        [JsonConverter(typeof(PersistentData.Loading.GenericObject))]
        public IObject Obj { get => obj; 
            set { 
                obj = value; 
                name = Obj.Name;
                guid = Obj.Guid;
            } 
        }
        [SerializeField]
        [JsonIgnore]
        private string guid;
        [SerializeField]
        [JsonIgnore]
        private string name;
        public float Preference;

        public Preferred(IObject preferredObj) : this(preferredObj, 0) { }
        public Preferred(IObject preferredObj, float preference)
        {
            Obj = new UnknownObject(preferredObj);
            this.Preference = preference;
            name = Obj.Name;
            guid = Obj.Guid;
        }
        public Preferred() : this(new UnknownObject()) { }
    }
}