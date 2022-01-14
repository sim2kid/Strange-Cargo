using System.Collections;
using PersistentData.Saving;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Component
{
    public abstract class Saveable : MonoBehaviour
    {
        public virtual GroupData Data { get; set; }

        protected List<ISaveable> _saveParts;

        protected virtual void Start()
        {
            _saveParts = new List<ISaveable>();
            _saveParts.Clear();
            _saveParts.AddRange(this.GetComponentsInChildren<ISaveable>(true));

            Saveable[] saveableChildren = GetComponentsInChildren<Saveable>(true);
            if (saveableChildren != null)
                if (saveableChildren.Length > 1)
                {
                    Console.LogError($"You should not keep children that can be save under other saveables. " +
                        $"One or more was found under {gameObject.name}. This saveable will be destoryed to prevent more errors.");
                    Destroy(this);
                }
        }

        public virtual void PreSerialization()
        {
            Data.ExtraData = new List<ISaveData>();
            foreach (var part in _saveParts)
            {
                part.PreSerialization();
                Data.ExtraData.Add(part.saveData);
            }
        }

        public virtual void PreDeserialization()
        {
            if (_saveParts == null)
                Start();
            foreach (var part in _saveParts)
                part.PreDeserialization();
        }

        public virtual void PostDeserialization()
        {
            if (_saveParts.Count != Data.ExtraData.Count)
            {
                Console.LogWarning("There are more save parts than componenets in object " + gameObject.name);
            }

            foreach (var part in Data.ExtraData)
            {
                ISaveable match = _saveParts.Find(x => x.saveData.GUID.Equals(part.GUID));
                if (match == null)
                {
                    Console.LogError($"Could not load part with GUID of '{part.GUID}' and type of '{part.DataType}'.");
                    continue;
                }
                match.PreDeserialization();
                match.saveData = part;
                match.PostDeserialization();
            }
        }
    }

}
