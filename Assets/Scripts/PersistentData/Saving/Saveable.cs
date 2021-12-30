using PersistentData.Loading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Saving
{
    [System.Serializable]
    public class Saveable : MonoBehaviour
    {
        public string PrefabResourceLocation;

        [SerializeField]
        private List<ISaveable> _saveParts = new List<ISaveable>();

        private void Start()
        {
            var saveBits = this.GetComponentsInChildren<ISaveable>(true);
            _saveParts.Clear();
            _saveParts.AddRange(saveBits);
        }

        public void PostDeserialization()
        {
            foreach(var part in _saveParts)
                part.PostDeserialization();
        }

        public void PreDeserialization()
        {
            foreach (var part in _saveParts)
                part.PreDeserialization();
        }

        public void PreSerialization()
        {
            foreach (var part in _saveParts)
                part.PreSerialization();
        }
    }
}