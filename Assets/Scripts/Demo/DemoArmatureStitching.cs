using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Siccity.GLTFUtility;
using System.IO;
using Utility;

public class DemoArmatureStitching : MonoBehaviour
{
    //Technical debt. This should be generated from a 'DNA' code and passed in
    [SerializeField]
    List<GameObject> bodyparts = new List<GameObject>();

    async void Start()
    {
        try
        {
            GameObject t1 = ImportGLTF("Assets/Models/GLTFs/Demo/Test1.gltf");
            GameObject t2 = ImportGLTF("Assets/Models/GLTFs/Demo/Test2.gltf");
            bodyparts.Add(t1);
            bodyparts.Add(t2);
            GameObject singleObj = ArmatureStitching.StitchObjects(bodyparts);
            singleObj.name = "Bobbert";
        }
        catch (Exception e) 
        {
            Debug.LogError($"Failed to load test files, retrying...\n{e}");
            await Task.Delay((int)(3 * 1000));
            //Start();
        }
    }

    GameObject ImportGLTF(string filepath)
    {
        return Importer.LoadFromFile(filepath);
    }

    // All other bones will get compiled down into 1 armature (found by name)
    // We will make a new Transform[] and put the compiled armature into it
    //      -- NOTE: The Transforms will need to have the right GameObject parent (found by name in out Transform[])

}
