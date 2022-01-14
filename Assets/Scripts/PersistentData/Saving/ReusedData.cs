using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace PersistentData.Saving
{
    [System.Serializable]
    public struct ReusedData : ISaveData
    {
        public string DataType => "Persistent";
        [JsonIgnore]
        public string _guid;
        public string GUID { get => _guid; set => _guid = value; }
        [JsonProperty(ItemConverterType = typeof(Loading.GetDataType))]
        public List<ISaveData> ExtraData;
    }
}