using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PersistentData.Saving;

namespace PersistentData
{
    [System.Serializable]
    public struct Save
    {
        public string GameVersion;
        public string SaveName;
        public long SaveTime;
        // Anything that can be recreated from a prefab
        public List<PrefabData> Prefabs;
        // Player (And the like)
        public List<ReusedData> Persistents;
        // Creatures
        public List<GroupData> Creatures;
    }
}