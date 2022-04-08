using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PersistentData.Saving;

namespace PersistentData.Component
{
    public class SavePlayerPosition : MonoBehaviour, ISaveable
    {
        [SerializeField]
        LocationData _location;

        GameObject eyes;

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(_location.GUID))
            {
                _location.GUID = System.Guid.NewGuid().ToString();
            }
        }

        public ISaveData saveData { get => _location; set { _location = (LocationData)value; } }

        private void Start()
        {
            eyes = GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().gameObject;
        }

        public void PostDeserialization()
        {
            transform.position = _location.Position;
            Vector3 rotation = _location.Rotation.eulerAngles;
            Vector3 eyesRotation = new Vector3(rotation.x, 0, rotation.z);
            Vector3 bodyRotation = new Vector3(0, rotation.y, 0);
            eyes.transform.localRotation = Quaternion.Euler(eyesRotation);
            transform.localRotation = Quaternion.Euler(bodyRotation);
            //SetGlobalScale(transform, _location.Scale);
        }

        public void PreDeserialization()
        {
            return;
        }

        public void PreSerialization()
        {
            _location.Position = transform.position;

            Vector3 eyesRotation = eyes.transform.localRotation.eulerAngles;
            Vector3 bodyRotation = transform.localRotation.eulerAngles;
            Vector3 compoundRotation = new Vector3(eyesRotation.x, bodyRotation.y, eyesRotation.z);
            _location.Rotation = Quaternion.Euler(compoundRotation);


            //_location.Scale = transform.lossyScale;
        }

        public static void SetGlobalScale(Transform transform, Vector3 globalScale)
        {
            transform.localScale = Vector3.one;
            transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
        }
    }
}