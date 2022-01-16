using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Saving
{
    [System.Serializable]
    public struct IsActiveData : ISaveData
    {
        public string DataType => "IsActive";
        [JsonIgnore]
        public string _guid;
        public string GUID { get => _guid; set => _guid = value; }
        [JsonIgnore]
        public bool _isActive;
        public bool IsActive { get => _isActive; set => _isActive = value; }
    }
}