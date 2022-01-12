using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Saving
{
    public class CreatureData : ISaveData
    {
        public string DataType => "Creature";
        [JsonIgnore]
        public string _guid;
        public string GUID { get => _guid; set => _guid = value; }
    }
}