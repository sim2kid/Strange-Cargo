using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PersistentData.Saving;

namespace PersistentData
{
    [System.Serializable]
    public struct SaveMeta
    {
        public string SaveName;
        public string GameVersion;
        public string SaveGuid;
        public long SaveTime;
    }
}