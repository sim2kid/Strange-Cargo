using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace PersistentData.Saving
{
    [System.Serializable]
    public class ReusedData : GroupData, ISaveData
    {
        public override string DataType => "Persistent";
    }
}