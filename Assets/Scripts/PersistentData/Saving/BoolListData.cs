using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Saving
{
    [System.Serializable]
    public class BoolListData : ISaveData
    {
        public string DataType => "BoolList";
        [JsonIgnore]
        public string _guid;
        public string GUID { get => _guid; set => _guid = value; }

        public Dictionary<string, bool> BoolData = new Dictionary<string, bool>();
    }
}