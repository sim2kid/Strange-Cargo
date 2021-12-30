using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Saving
{
    public class SavePosition : MonoBehaviour, ISaveable
    {
        public bool PostDeserialization()
        {
            throw new System.NotImplementedException();
        }

        public bool PreSerialization()
        {
            throw new System.NotImplementedException();
        }
    }
}