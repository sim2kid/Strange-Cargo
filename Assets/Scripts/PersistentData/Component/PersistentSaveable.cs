using System.Collections;
using PersistentData.Saving;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Component
{
    public class PersistentSaveable : Saveable
    {
        [SerializeField]
        public ReusedData data;

        public override GroupData Data { get => data; set => data = (ReusedData)value; }

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(data._guid))
            {
                data._guid = System.Guid.NewGuid().ToString();
            }
        }
    }
}