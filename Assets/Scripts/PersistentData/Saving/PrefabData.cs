using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Saving
{
    [System.Serializable]
    public class PrefabData : ISaveData
    {
        [JsonIgnore]
        public string DataType => "Prefab";
        public string GUID { get; set; }
        public string PrefabResourceLocation;
        [JsonProperty(ItemConverterType = typeof(Loading.GetDataType))]
        public List<ISaveData> ExtraData;
    }
}