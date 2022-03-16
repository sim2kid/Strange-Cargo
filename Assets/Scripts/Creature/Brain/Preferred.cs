using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Environment;
using Newtonsoft.Json;

namespace Creature.Brain
{
    [System.Serializable]
    public class Preferred
    {
        public UnspecifiedTask TaskObj { get; set; }

        [JsonIgnore]
        public string Guid => TaskObj.Obj.Guid;

        // For unity Editor vvv

        [JsonIgnore, SerializeField]
        private string _objName;
        [JsonIgnore, SerializeField]
        private string _guid;
        [JsonIgnore, SerializeField]
        private string _taskName;

        public void Update() 
        {
            _objName = TaskObj.Obj.Name;
            _guid = TaskObj.Obj.Guid;
            _taskName = TaskObj.TaskName;
        }

        // For unity Editor ^^^

        public float Preference;

        public Preferred(ITaskObject preferredTaskObj) : this(preferredTaskObj, 0) { }
        public Preferred(ITaskObject preferredTaskObj, float preference)
        {
            TaskObj = new UnspecifiedTask(preferredTaskObj);
            this.Preference = preference;

            // For the editor
            Update();
        }
        public Preferred() : this(new UnspecifiedTask()) { }

        public Preferred(IObject preferredObj, string taskName) : this(new UnspecifiedTask(preferredObj, taskName)) { }

        public Preferred(IObject preferredObj, string taskName, float preferance) 
            : this(new UnspecifiedTask(preferredObj, taskName), preferance) { }
    }
}