using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Saving {
    [System.Serializable]
    public class ColorData : ISaveData
    {
        [SerializeField]
        public string DataType => "RandomColor";
        [SerializeField]
        public string Color;
        [SerializeField]
        [JsonIgnore]
        public string _guid;
        [SerializeField]
        public string GUID => _guid;
    }
}