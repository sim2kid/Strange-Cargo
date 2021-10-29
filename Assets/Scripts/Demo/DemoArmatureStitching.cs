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
    List<GameObject> bodyparts = new List<GameObject>();


    async void Start()
    {
        Debug.Log(Application.streamingAssetsPath);
        try
        {
            bodyparts.Add(ImportGLTF(Path.Combine(Application.streamingAssetsPath, "Models/Bodies/BeanBody.gltf")));
            bodyparts.Add(ImportGLTF(Path.Combine(Application.streamingAssetsPath, "Models/Ears/FluffyEars.gltf")));
            bodyparts.Add(ImportGLTF(Path.Combine(Application.streamingAssetsPath, "Models/Heads/BobbleHead.gltf")));
            bodyparts.Add(ImportGLTF(Path.Combine(Application.streamingAssetsPath, "Models/Horns/PointyHorns.gltf")));
            bodyparts.Add(ImportGLTF(Path.Combine(Application.streamingAssetsPath, "Models/Legs/HoovedLegs.gltf")));
            bodyparts.Add(ImportGLTF(Path.Combine(Application.streamingAssetsPath, "Models/Tails/PointyTail.gltf")));
            //bodyparts.Add(ImportGLTF(Path.Combine(Application.streamingAssetsPath, "/Models/Demo/demo.gltf")));

            GameObject singleObj = ArmatureStitching.StitchObjects(bodyparts);
            singleObj.name = "Strubie";
            singleObj.transform.localScale = Vector3.one * 0.1f;
            singleObj.AddComponent<TextureConverter.TextureController>();

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
}
