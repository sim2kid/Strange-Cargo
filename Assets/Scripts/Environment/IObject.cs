using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    public interface IObject : IEquatable<IObject>
    {
        public string Name { get; set; }
        public string Guid { get; }
    }
}