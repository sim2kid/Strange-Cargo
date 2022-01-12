using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Saving
{
    public interface ISaveData
    {
        public string DataType { get; }
        public string GUID { get; set; }
    }
}