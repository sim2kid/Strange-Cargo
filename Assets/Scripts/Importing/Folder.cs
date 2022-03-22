using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Importing
{
    [System.Serializable]
    public class Folder
    {
        /// <summary>
        /// The path to this folder from the Top Level Location. Eg: "Face/Generic"
        /// </summary>
        public string FolderName;
        /// <summary>
        /// List of files in this folder. Does not include nested folders.
        /// </summary>
        public List<File> Files = new List<File>();
    }
}