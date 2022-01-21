using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Saving
{
    [System.Serializable]
    public class GroupData : ISaveData
    {
        public virtual string DataType => "Data";
        [JsonIgnore]
        public string _guid;
        public virtual string GUID { get => _guid; set => _guid = value; }
        [JsonProperty(ItemConverterType = typeof(Loading.GetDataType))]
        public List<ISaveData> ExtraData;
    }
}