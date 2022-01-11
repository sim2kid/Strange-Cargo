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
        public List<PrefabData> Prefabs;
    }
}