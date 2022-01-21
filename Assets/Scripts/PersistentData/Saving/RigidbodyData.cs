using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace PersistentData.Saving
{
    [System.Serializable]
    public struct RigidbodyData : ISaveData
    {
        public string DataType => "Rigidbody";
        [JsonIgnore]
        public string _guid;
        public string GUID { get => _guid; set => _guid = value; }

        [JsonIgnore]
        public RigidbodyConstraints Constraints { get => (RigidbodyConstraints)_constraints; 
            set => _constraints = (int)value; }

        [JsonIgnore]
        public Vector3 Velocity { get => new Vector3(_velocity[0], _velocity[1], _velocity[2]);
            set => _velocity = new float[] { value.x, value.y, value.z };
        }

        [JsonIgnore]
        public Vector3 Rotation
        {
            get => new Vector3(_rotation[0], _rotation[1], _rotation[2]);
            set => _rotation = new float[] { value.x, value.y, value.z };
        }

        [JsonProperty]
        private int _constraints;
        [JsonProperty]
        private float[] _velocity;
        [JsonProperty]
        private float[] _rotation;
        public bool IsKinematic;
    }
}