using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Saving
{
    [System.Serializable]
    public class PrefabData : GroupData, ISaveData, System.IEquatable<PrefabData>
    {
        [JsonIgnore]
        public override string DataType => "Prefab";
        public string PrefabResourceLocation;

        public bool Equals(PrefabData other)
        {
            return this.PrefabResourceLocation == other.PrefabResourceLocation;
        }
    }
}