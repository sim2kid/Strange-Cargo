using System.Collections;
using PersistentData.Saving;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Component
{
    public class PersistentSaveable : MonoBehaviour
    {
        [SerializeField]
        public ReusedData data;

        List<ISaveable> _saveParts;

        private void Awake()
        {
            if (string.IsNullOrEmpty(data.GUID))
            {
                data.GUID = System.Guid.NewGuid().ToString();
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
            data.ExtraData = new List<ISaveData>();
            foreach (var part in _saveParts)
            {
                part.PreSerialization();
                data.ExtraData.Add(part.saveData);
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
            if (_saveParts.Count != data.ExtraData.Count)
            {
                Console.LogWarning("There are more save parts than componenets in object " + gameObject.name);
            }

            foreach (var part in data.ExtraData)
            {
                ISaveable match = _saveParts.Find(x => x.saveData.GUID.Equals(part.GUID));
                if (match == null)
                {
                    Console.LogError($"Could not load part with GUID of '{part.GUID}' and type of '{part.DataType}' " +
                        $"from prefab '{data.PrefabResourceLocation}' and GUID of '{data.GUID}'.");
                    continue;
                }
                match.PreDeserialization();
                match.saveData = part;
                match.PostDeserialization();
            }
        }
    }
}