using PersistentData.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Component
{
    public class PrefabSaveable : Saveable
    {
        [SerializeField]
        public PrefabData prefabData;

        public override GroupData Data { get => prefabData; set => prefabData = (PrefabData)value; }

        private void Awake()
        {
            if (string.IsNullOrEmpty(Data.GUID)) 
            {
                Data.GUID = System.Guid.NewGuid().ToString();
            }
        }

        public override void PostDeserialization()
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