using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Importing
{
    [System.Serializable]
    public class Folder : ScriptableObject
    {
        public string FolderName;
        public List<File> Files = new List<File>();
    }
}