using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Saving
{
    public class CreatureData : ISaveData
    {
        [SerializeField]
        public string DataType => "Creature";
        [SerializeField]
        [Newtonsoft.Json.JsonIgnore]
        public string _guid;
        [SerializeField]
        public string GUID => _guid;
    }
}