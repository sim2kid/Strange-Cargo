using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Saving {
    [System.Serializable]
    public class ColorData : ISaveData
    {
        public string DataType => "RandomColor";
        public string Color;
        public string GUID { get; set; }
    }
}