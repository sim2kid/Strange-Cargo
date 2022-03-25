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
        public UnknownObject Obj { get; set; }
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
            Obj = new UnknownObject(taskObject.Obj);
            TaskName = taskObject.TaskName;
        }

        public UnspecifiedTask(IObject obj, string taskName)
        {
            Obj = new UnknownObject(obj);
            TaskName = taskName;
        }
    }
}