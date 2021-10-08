using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public static class ArmatureStitching
    {
        /// <summary>
        /// This takes the imported armatures (just the root) and stitches them together.
        /// </summary>
        /// <param name="gameObjects"></param>
        /// <returns></returns>
        public static GameObject StitchObjects(List<GameObject> gameObjects) 
        {
            // Make sure we're working with some objects
            if(gameObjects.Count <= 0) 
                return null;

            // This is our new root gameobject. This is what we will return as well
            GameObject baseArmature = new GameObject();
            List<Transform> knownBones = new List<Transform>();

            // We loop though all the armatures
            foreach (GameObject obj in gameObjects) 
            {
                // Gather known bones
                Transform throwawayBone = GetArmature(obj, knownBones);
                foreach (Transform babyBone in GetBones(throwawayBone)) 
                    if (babyBone.parent.name == throwawayBone.name) 
                        babyBone.parent = baseArmature.transform;
                // Don't keep the throwaway bone
                GameObject.Destroy(throwawayBone.gameObject);

                // We grab and loop through the bodyparts (not bones but models)
                foreach (GameObject bodyPart in GetModelGameObjects(obj))
                {
                    // We put the bodypart set to the base armature.
                    bodyPart.transform.SetParent(baseArmature.transform);
                }

                // Then we destroy the unused models
                GameObject.Destroy(obj);
            }

            // We then loop through one more time and assign the bones for each model
            foreach (GameObject bodyPart in GetModelGameObjects(baseArmature)) 
                bodyPart.GetComponent<SkinnedMeshRenderer>().bones = knownBones.ToArray(); // TO-DO Fill this in! or no animations will work!


            return baseArmature;
        }

        /// <summary>
        /// This will recursivly loop through the <paramref name="rootObj"/>'s transform component to find all models with SkinnedMeshRenderers.
        /// </summary>
        /// <param name="rootObj"></param>
        /// <returns></returns>
        private static List<GameObject> GetModelGameObjects(GameObject rootObj) 
        {
            List<GameObject> models = new List<GameObject>();
            for (int i = 0; i < rootObj.transform.childCount; i++) 
            {
                GameObject temp = rootObj.transform.GetChild(i).gameObject;
                if (temp.GetComponent<SkinnedMeshRenderer>() != null)
                    models.Add(temp);
                if(temp.transform.childCount > 0)
                    foreach(GameObject obj in GetModelGameObjects(temp))
                        models.Add(obj);
            }
            return models;
        }

        /// <summary>
        /// Returns a list of all the children transforms
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        private static List<Transform> GetBones(Transform parent) 
        {
            List<Transform> children = new List<Transform>();
            for (int i = 0; i < parent.childCount; i++) 
            {
                Transform child = parent.GetChild(i);
                children.Add(child);
                if(parent.childCount > 0)
                    foreach(Transform baby in GetBones(child))
                        children.Add(baby);
            }
            return children;
        }

        /// <summary>
        /// Boils down an an Armature to a single parent
        /// </summary>
        /// <param name="rootObj"></param>
        /// <param name="knownBones"></param>
        /// <returns></returns>
        private static Transform GetArmature(GameObject rootObj, List<Transform> knownBones) 
        {
            Transform BaseBone = new GameObject().transform;
            BaseBone.name = "Returned Armature";
            if (knownBones == null)
                knownBones = new List<Transform>();
            for (int i = 0; i < rootObj.transform.childCount; i++) 
            {
                GameObject bone = rootObj.transform.GetChild(i).gameObject;

                // If it has a meshrenderer it's not a bone
                if (bone.GetComponent<SkinnedMeshRenderer>() != null)
                    continue;

                // Check if bone is in the KnownBones
                if (knownBones.Find(x => x.name == bone.name) == null) 
                {
                    Transform parentBone = knownBones.Find(x => (x.name == bone.transform.parent.name));
                    if (parentBone == null)
                    {
                        bone.transform.SetParent(BaseBone);
                    }
                    else
                    {
                        bone.transform.SetParent(parentBone);
                    }
                    knownBones.Add(bone.transform);

                }

                // Repeat on all children bones
                if (bone.transform.childCount > 0) {
                    Transform throwawayBone = GetArmature(bone, knownBones);
                    foreach (Transform babyBone in GetBones(throwawayBone))
                        knownBones.Add(babyBone);
                    // Don't keep the throwaway bone
                    GameObject.Destroy(throwawayBone.gameObject);
                }
            }
            return BaseBone;
        }

    }
}
