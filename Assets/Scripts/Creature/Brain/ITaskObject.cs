using Environment;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Brain 
{
    public interface ITaskObject : IEquatable<ITaskObject>
    {
        public UnknownObject Obj { get; set; }
        public string TaskName { get; set; }
    }
}