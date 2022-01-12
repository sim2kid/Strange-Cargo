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
            if (string.IsNullOrWhiteSpace(_location.GUID)) 
            {
                _location.GUID = System.Guid.NewGuid().ToString();
            }
        }


        public ISaveData saveData { get => _location; set { _location = (LocationData)value; } }

        public void PostDeserialization()
        {
            transform.position = _location.Position;
            transform.rotation = _location.Rotation;
            SetGlobalScale(transform, _location.Scale);
        }

        public void PreDeserialization()
        {
            return;
        }

        public void PreSerialization()
        {
            _location.Position = transform.position;
            _location.Rotation = transform.rotation;
            _location.Scale = transform.lossyScale;
        }

        public static void SetGlobalScale(Transform transform, Vector3 globalScale)
        {
            transform.localScale = Vector3.one;
            transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
        }
    }
}