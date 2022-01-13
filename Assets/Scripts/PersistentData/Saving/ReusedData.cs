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
        public string GUID { get; set; }
        public string PrefabResourceLocation;
        [JsonProperty(ItemConverterType = typeof(Loading.GetDataType))]
        public List<ISaveData> ExtraData;
    }
}