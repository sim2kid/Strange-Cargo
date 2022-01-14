using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Saving {
    [System.Serializable]
    public struct TimeData : ISaveData
    {
        [JsonIgnore]
        public string _guid;
        public string GUID { get => _guid; set => _guid = value; }
        public string DataType => "Time";
        [HideInInspector]
        public float Time;
    }
}