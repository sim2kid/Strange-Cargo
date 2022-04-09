using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace PersistentData.Saving
{
    [System.Serializable]
    public class RigidbodyData : ISaveData
    {
        public string DataType => "Rigidbody";
        [JsonIgnore]
        public string _guid;
        public string GUID { get => _guid; set => _guid = value; }

        [JsonIgnore]
        public RigidbodyConstraints Constraints { get => (RigidbodyConstraints)_constraints;
            set => _constraints = (int)value; }

        [JsonIgnore]
        public Vector3 Velocity {
            get 
            {
                if (_velocity == null) _velocity = new float[3];
                return new Vector3(_velocity[0], _velocity[1], _velocity[2]); 
            }
            set 
            { 
                if(value != null) 
                {
                    _velocity = new float[] { value.x, value.y, value.z };
                } 
            }
        }

        [JsonIgnore]
        public Vector3 Rotation
        {
            get
            {
                if (_rotation == null) _rotation = new float[3];
                return new Vector3(_rotation[0], _rotation[1], _rotation[2]);
            }
            set
            {
                if (value != null)
                {
                    _rotation = new float[] { value.x, value.y, value.z };
                }
            }
        }

        [JsonProperty]
        private int _constraints;
        [JsonProperty]
        private float[] _velocity = new float[3];
        [JsonProperty]
        private float[] _rotation = new float[3];
        public bool IsKinematic;
    }
}