using PersistentData.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Component
{
    public class SaveLocation : MonoBehaviour, ISaveable
    {
        [SerializeField]
        LocationData _location;

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(_location._guid)) 
            {
                _location._guid = System.Guid.NewGuid().ToString();
            }
        }


        public ISaveData saveData { get => _location; set { _location = (LocationData)value; } }

        public void PostDeserialization()
        {
            transform.localPosition = _location.Position;
            transform.localRotation = _location.Rotation;
            transform.localScale = _location.Scale;
        }

        public void PreDeserialization()
        {
            return;
        }

        public void PreSerialization()
        {
            _location.Position = transform.localPosition;
            _location.Rotation = transform.localRotation;
            _location.Scale = transform.localScale;
        }
    }
}