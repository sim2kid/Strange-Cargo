using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Importing;
using System.Linq;

public class DemoImporter : MonoBehaviour
{
    public string FileLocation;
    public string SearchPattern;
    // Start is called before the first frame update
    void Start()
    {
        Database db = Importer.Import(FileLocation, null, null, SearchPattern);
        List<Folder> folders = db.Folders.Values.ToList<Folder>();
        foreach (Folder folder in folders) 
        {
            foreach (File file in folder.Files) 
            {
                Debug.Log($"{file.ParentFolder} | {file.FileLocation}");
            }
        }
    }
}
