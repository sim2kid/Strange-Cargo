using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PersistentData.Loading;

namespace PersistentData.Saving
{
    public interface ISaveable
    {
        public string PrefabResourceLocation { get; }
        public ILoadable Loadable { get; }
        public bool PreSerialization();
        public bool PreDeserialization();
        public bool PostDeserialization();

    }
}