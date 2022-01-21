using PersistentData.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Component
{
    [RequireComponent(typeof(Creature.CreatureController))]
    [RequireComponent(typeof(SaveCreaturePosition))]
    public class CreatureSaveable : Saveable
    {
        [SerializeField]
        GroupData data = new GroupData();
        public override GroupData Data { get => data; set => data = value; }
        private void Awake()
        {
            if (string.IsNullOrEmpty(data.GUID))
            {
                data.GUID = System.Guid.NewGuid().ToString();
            }
        }
    }
}