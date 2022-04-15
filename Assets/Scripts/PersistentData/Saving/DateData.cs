using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace PersistentData.Saving
{
    [System.Serializable]
    public struct DateData : ISaveData
    {
        [JsonIgnore]
        public string _guid;
        public string GUID { get => _guid; set => _guid = value; }
        public string DataType => "Date";
        [HideInInspector]
        public float Day;
        [HideInInspector]
        public float Month;
    }
}
