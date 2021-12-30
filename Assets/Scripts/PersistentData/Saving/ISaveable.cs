using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PersistentData.Loading;

namespace PersistentData.Saving
{
    public interface ISaveable
    {
        public ILoadable Loadable { get; }
        public void PreSerialization();
        public void PreDeserialization();
        public void PostDeserialization();

    }
}