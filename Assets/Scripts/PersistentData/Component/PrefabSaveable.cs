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
        public PrefabData _prefabData;

        List<ISaveable> _saveParts;

        private void Awake()
        {
            if (string.IsNullOrEmpty(_prefabData.GUID)) 
            {
                _prefabData.GUID = System.Guid.NewGuid().ToString();
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
            _prefabData.ExtraData = new List<ISaveData>();
            foreach (var part in _saveParts)
            {
                part.PreSerialization();
                _prefabData.ExtraData.Add(part.saveData);
            }
        }

        public void PreDeserialization()
        {
            foreach (var part in _saveParts)
                part.PreDeserialization();
        }

        public void PostDeserialization()
        {
            if (_saveParts.Count != _prefabData.ExtraData.Count) 
            {
                Console.LogWarning("There are more save parts than componenets in object " + gameObject.name);
            }

            foreach (var part in _prefabData.ExtraData)
            {
                ISaveable match = _saveParts.Find(x => x.saveData.GUID.Equals(part.GUID));
                if (match == null) 
                {
                    Console.LogError("Could not load part with GUID of '" + part.GUID + "' and type of '" + part.DataType + "'.");
                    continue;
                }
                match.saveData = part;
                match.PreDeserialization();
            }
            for (int i = 0; i < _saveParts.Count && i < _prefabData.ExtraData.Count; i++)
            {
                _saveParts[i].saveData = _prefabData.ExtraData[i];
                _saveParts[i].PostDeserialization();
            }
        }

        

        
    }
}