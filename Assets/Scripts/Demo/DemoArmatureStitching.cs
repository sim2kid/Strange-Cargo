using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoArmatureStitching : MonoBehaviour
{
    private SkinnedMeshRenderer skinnedMeshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        // SHOW ME DA BONES
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer != null) 
        {
            Debug.Log("SHOW ME DA BONES!");
            foreach (Transform bone in skinnedMeshRenderer.bones) 
            {
                Debug.Log(bone.ToString());
            }
        }

        // Get list of Models
        // Select first model as the base model (maybe rename the parent game object)
        // Split the model into objects and armatures
        // Objects have a SkinnedMeshRenderer component
        // All other bones will get compiled down into 1 armature (found by name)
        // We will make a new Transform[] and put the compiled armature into it
        //      -- NOTE: The Transforms will need to have the right GameObject parent (found by name in out Transform[])
        // Then we go through each SkinnedMeshRenderer and set SkinnedMeshRenderer#bones to our Transform[]
        // Then that's our finished model! We can delete everything else!
    }
}
