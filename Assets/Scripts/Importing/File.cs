using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Importing
{
    [System.Serializable]
    public class File
    {
        public string ParentFolder;
        public string FileLocation;

        public string FileName 
        { 
            get
            {
                string[] allparts = FileLocation.Split('\\');
                return allparts[allparts.Length-1];
            }
        }
    }
}