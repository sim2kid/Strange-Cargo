using PersistentData.Loading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Saving
{
    [System.Serializable]
    public class LocationData : ISaveData
    {
        [Newtonsoft.Json.JsonIgnore]
        public Vector3 Position 
        {
            get 
            {
                return new Vector3(_position[0], _position[1], _position[2]);
            }
            set 
            {
                _position = new float[]{ value.x, value.y, value.z };
            }
        }
        [Newtonsoft.Json.JsonIgnore]
        public Quaternion Rotation
        {
            get
            {
                return new Quaternion(_rotation[0], _rotation[1], _rotation[2], _rotation[3]);
            }
            set
            {
                _rotation = new float[] { value.x, value.y, value.z, value.w };
            }
        }
        [Newtonsoft.Json.JsonIgnore]
        public Vector3 Scale
        {
            get
            {
                return new Vector3(_scale[0], _scale[1], _scale[2]);
            }
            set
            {
                _scale = new float[] { value.x, value.y, value.z };
            }
        }

        [SerializeField]
        [Newtonsoft.Json.JsonIgnore]
        public string _guid;
        [SerializeField]
        public string GUID => _guid;
        
        public string DataType => "Location";

        [Newtonsoft.Json.JsonProperty]
        private float[] _position;
        [Newtonsoft.Json.JsonProperty]
        private float[] _rotation;
        [Newtonsoft.Json.JsonProperty]
        private float[] _scale;

    }

}