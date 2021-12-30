using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Saving
{
    public interface ISaveable
    {
        public bool PreSerialization();
        public bool PostDeserialization();

    }
}