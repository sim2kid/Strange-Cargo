using PersistentData.Loading;
using PersistentData.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Component
{
    public class PrefabSaveable : MonoBehaviour
    {
        [SerializeField]
        public PrefabData prefabData;

        List<ISaveable> _saveParts;

        private void Awake()
        {
            if (string.IsNullOrEmpty(prefabData.GUID)) 
            {
                prefabData.GUID = System.Guid.NewGuid().ToString();
            }
        }

        private void Start()
        {
            _saveParts = new List<ISaveable>();
            _saveParts.Clear();
            _saveParts.AddRange(this.GetComponentsInChildren<ISaveable>(true));
        }

        public void PreSerialization()
        {
            prefabData.ExtraData = new List<ISaveData>();
            foreach (var part in _saveParts)
            {
                part.PreSerialization();
                prefabData.ExtraData.Add(part.saveData);
            }
        }

        public void PreDeserialization()
        {
            if (_saveParts == null)
                Start();
            foreach (var part in _saveParts)
                part.PreDeserialization();
        }

        public void PostDeserialization()
        {
            if (_saveParts.Count != prefabData.ExtraData.Count) 
            {
                Console.LogWarning("There are more save parts than componenets in object " + gameObject.name);
            }

            foreach (var part in prefabData.ExtraData)
            {
                ISaveable match = _saveParts.Find(x => x.saveData.GUID.Equals(part.GUID));
                if (match == null) 
                {
                    Console.LogError($"Could not load part with GUID of '{part.GUID}' and type of '{part.DataType}' " +
                        $"from prefab '{prefabData.PrefabResourceLocation}' and GUID of '{prefabData.GUID}'.");
                    continue;
                }
                match.PreDeserialization();
                match.saveData = part;
                match.PostDeserialization();
            }
        }
    }
}