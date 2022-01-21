using Creature.Stats;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Saving
{
    [System.Serializable]
    public class NeedSourceData : ISaveData
    {
        [JsonIgnore]
        public string _guid;
        public string GUID { get => _guid; set => _guid = value; }
        public string DataType => "BasicNeedSource";
        [HideInInspector]
        public Needs BaseNeeds;
        [HideInInspector]
        public float Fullness;
    }
}