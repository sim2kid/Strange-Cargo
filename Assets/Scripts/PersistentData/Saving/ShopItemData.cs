using Newtonsoft.Json;
using PersistentData.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Saving
{
    [System.Serializable]
    public class ShopItemData : ISaveData
    {
        public string DataType => "ShopItem";
        [JsonIgnore]
        public string _guid;
        public string GUID { get => _guid; set => _guid = value; }

        public float Price;
        public string Name;
        public string Description;
    }
}
