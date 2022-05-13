using PersistentData.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Component
{
    public class ShopItemMetadata : MonoBehaviour, ISaveable
    {
        [SerializeField]
        ShopItemData _itemData;

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(_itemData.GUID))
            {
                _itemData.GUID = "7cdb6b2a-804e-4830-9312-42270e8d191a";
            }
        }

        public ISaveData saveData { get => _itemData; set { _itemData = (ShopItemData)value; } }

        public void PostDeserialization()
        {
            return;
        }

        public void PreDeserialization()
        {
            return;
        }

        public void PreSerialization()
        {
            return;
        }
    }
}