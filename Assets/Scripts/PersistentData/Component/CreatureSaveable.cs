using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Component
{
    public class CreatureSaveable : Saveable
    {
        private void Awake()
        {
            if (string.IsNullOrEmpty(Data.GUID))
            {
                Data.GUID = System.Guid.NewGuid().ToString();
            }
        }
    }
}