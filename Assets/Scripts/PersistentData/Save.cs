using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PersistentData.Saving;

namespace PersistentData
{
    public struct Save
    {
        public string GameVersion;
        public string SaveName;
        public string SaveTime;
        List<ISaveable> SavedObjects;
    }
}