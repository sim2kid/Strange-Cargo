using Environment;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Brain
{
    [System.Serializable]
    public class UnspecifiedTask : ITaskObject
    {
        [JsonConverter(typeof(PersistentData.Loading.GenericObject))]
        public IObject Obj { get; set; }
        public string TaskName { get; set; }

        public bool Equals(ITaskObject other) 
        {
            return Obj.Equals(other.Obj) && TaskName.Equals(other.TaskName);
        }

        public UnspecifiedTask() 
        {
            Obj = new UnknownObject();
            TaskName = string.Empty;
        }

        public UnspecifiedTask(ITaskObject taskObject) 
        {
            Obj = taskObject.Obj;
            TaskName = taskObject.TaskName;
        }
    }
}