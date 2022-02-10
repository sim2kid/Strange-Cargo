using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace PersistentData.Saving
{
    [System.Serializable]
    public class ItemData : ISaveData
    {
        public string DataType => "Item";
        [JsonIgnore]
        public string _guid;
        public string GUID { get => _guid; set => _guid = value; }
        
        public RigidbodyData rigidbodyData;
        public PrefabData prefabData;
    }
}