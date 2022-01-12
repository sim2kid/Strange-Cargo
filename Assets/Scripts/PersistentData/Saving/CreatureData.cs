using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Saving
{
    public class CreatureData : ISaveData
    {
        public string DataType => "Creature";
        public string GUID { get; set; }
    }
}