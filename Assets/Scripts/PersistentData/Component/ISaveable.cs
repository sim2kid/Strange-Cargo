using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PersistentData.Saving;

namespace PersistentData.Component
{
    public interface ISaveable
    {
        public ISaveData saveData { get; set; }
        public void PreSerialization();
        public void PreDeserialization();
        public void PostDeserialization();
    }
}