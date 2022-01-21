using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    [System.Serializable]
    public class UnknownObject : IObject
    {
        public string Name { get; set; }

        public string Guid { get; set; }

        public bool Equals(IObject other)
        {
            return other.Guid.Equals(Guid);
        }

        public UnknownObject(IObject other) 
        {
            Name = other.Name;
            Guid = other.Guid;
        }

        public UnknownObject() 
        {
            Name = string.Empty;
            Guid = string.Empty;
        }
    }
}