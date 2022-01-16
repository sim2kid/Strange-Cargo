using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Saving
{
    [System.Serializable]
    public class PrefabData : GroupData, ISaveData
    {
        [JsonIgnore]
        public override string DataType => "Prefab";
        public string PrefabResourceLocation;
    }
}