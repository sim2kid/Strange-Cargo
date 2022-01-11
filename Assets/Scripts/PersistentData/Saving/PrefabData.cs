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
        [SerializeField]
        public string DataType => "Prefab";
        [SerializeField]
        public string GUID { get; set; }
        [SerializeField]
        public string PrefabResourceLocation;
        [SerializeField]
        [JsonProperty(ItemConverterType = typeof(Loading.GetDataType))]
        public List<ISaveData> ExtraData;
    }
}