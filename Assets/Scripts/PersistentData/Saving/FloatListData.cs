using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Saving
{
    [System.Serializable]
    public class FloatListData : ISaveData
    {
        public string DataType => "FloatList";
        [JsonIgnore]
        public string _guid;
        public string GUID { get => _guid; set => _guid = value; }
        public Dictionary<string, float> FloatData = new Dictionary<string, float>();
    }
}