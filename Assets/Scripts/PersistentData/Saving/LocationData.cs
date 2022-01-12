using Newtonsoft.Json;
using PersistentData.Loading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Saving
{
    [System.Serializable]
    public class LocationData : ISaveData
    {
        [JsonIgnore]
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
        [JsonIgnore]
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
        [JsonIgnore]
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
        [JsonIgnore]
        public string _guid;
        public string GUID { get => _guid; set => _guid = value; }

        public string DataType => "Location";

        [JsonProperty]
        private float[] _position;
        [JsonProperty]
        private float[] _rotation;
        [JsonProperty]
        private float[] _scale;

    }

}