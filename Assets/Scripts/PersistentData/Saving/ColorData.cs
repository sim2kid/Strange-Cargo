using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Saving {
    [System.Serializable]
    public class ColorData : ISaveData
    {
        public string DataType => "RandomColor";
        public string Color;
        [JsonIgnore]
        public string _guid;
        public string GUID { get => _guid; set => _guid = value; }
    }
}