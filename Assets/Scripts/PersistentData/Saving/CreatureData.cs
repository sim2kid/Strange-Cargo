using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Saving
{
    [System.Serializable]
    public struct CreatureData : ISaveData
    {
        public string DataType => "Creature";
        [JsonIgnore]
        public string _guid;
        public string GUID { get => _guid; set => _guid = value; }
        public Genetics.DNA dna;
        public Creature.Stats.Needs needs;
        public string frontFeetSound;
        public string backFeetSound;
        public Creature.Brain.BasicBrain brain;
    }
}