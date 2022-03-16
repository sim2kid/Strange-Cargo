using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Importing
{
    [System.Serializable]
    public class File
    {
        /// <summary>
        /// The parent directory for the file. This is from the Top Level Folder as well.
        /// This may not be the same as the file location. Eg: "Generic/Music"
        /// </summary>
        public string ParentFolder;
        /// <summary>
        /// File Location from Top Level Folder. Eg: "Face/Generic/Image.png"
        /// </summary>
        public string FileLocation;
        /// <summary>
        /// The File name with Extension. 
        /// This may not be the actual file, but just the identity of the file. Eg: "SomeFile.png"
        /// </summary>
        public string FileName;
    }
}