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

        public Dictionary<string, bool> BoolData { get; set; } = new Dictionary<string, bool>();

        [JsonProperty]
        private List<string> BoolName { get; set; } = new List<string>();
        [JsonProperty]
        private List<bool> BoolValue { get; set; } = new List<bool>();

        public void PreSerialize() 
        {
            BoolName.Clear();
            BoolValue.Clear();
            foreach (var item in BoolData) 
            {
                BoolName.Add(item.Key);
                BoolValue.Add(item.Value);
            }
        }

        public void PostDeserialize() 
        {
            BoolData.Clear();
            for (int i = 0; i < BoolName.Count && i < BoolValue.Count; i++) 
            {
                BoolData.Add(BoolName[i], BoolValue[i]);
            }
        }
    }
}