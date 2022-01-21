using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PersistentData.Saving;
using UnityEngine.AI;

namespace PersistentData.Component
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class SaveCreaturePosition : MonoBehaviour, ISaveable
    {
        [SerializeField]
        LocationData _location;

        NavMeshAgent nav;

        private void Start()
        {
            nav = GetComponent<NavMeshAgent>();
        }

        private void Awake()
        {
            if (string.IsNullOrWhiteSpace(_location.GUID))
            {
                _location.GUID = "72f61c38-97ee-4205-91ea-e272cb31319d";
            }
        }

        public ISaveData saveData { get => _location; set { _location = (LocationData)value; } }

        public void PostDeserialization()
        {
            nav = GetComponent<NavMeshAgent>();
            if (nav != null)
                if(!nav.Warp(_location.Position))
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