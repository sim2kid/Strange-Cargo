using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Importing
{
    [System.Serializable]
    public class File : ScriptableObject
    {
        public string ParentFolder;
        public string FileLocation;
    }
}