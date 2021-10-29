using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
            if(gameObjects.Count == 0)
                return null;

            GameObject baseObj = gameObjects[0];

            for (int i = 1; i < gameObjects.Count; i++) 
            {
                AddLimb(gameObjects[i], baseObj);
                GameObject.Destroy(gameObjects[i]);
            }
            return baseObj;
        }

        /// <summary>
        /// This will copy the Meshs from <paramref name="Limb"/> and put them on the <paramref name="Body"/> gameobject
        /// </summary>
        /// <param name="Limb"></param>
        /// <param name="Body"></param>
        public static void AddLimb(GameObject Limb, GameObject Body) 
        {
            SkinnedMeshRenderer[] BonedObjs = Limb.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer renderer in BonedObjs) 
            {
                ProcessBonedObject(renderer, Body);
            }
        }

        private static void ProcessBonedObject(SkinnedMeshRenderer renderer, GameObject root) 
        {
            // Create SubObj
            GameObject newObj = new GameObject(renderer.name);
            newObj.transform.SetParent(root.transform);
            // Add renderer
            SkinnedMeshRenderer newRenderer = newObj.AddComponent<SkinnedMeshRenderer>();
            // Assemble Bones
            Transform[] myBones = new Transform[renderer.bones.Length];
            for (int i = 0; i < renderer.bones.Length; i++) 
            {
                myBones[i] = FindChildByName(renderer.bones[i].name, root.transform);
            }
            // Assemble Renderer
            newRenderer.bones = myBones;
            newRenderer.sharedMesh = renderer.sharedMesh;
            newRenderer.materials = renderer.materials;

            //Copy other components besides the renderers
            Component[] components = renderer.GetComponents<Component>();
            foreach (Component c in components) 
            {
                if (c.GetType() != typeof(SkinnedMeshRenderer) && c.GetType() != typeof(Transform)) 
                {
                    MethodInfo method = typeof(ArmatureStitching).GetMethod(nameof(ArmatureStitching.CopyValues));
                    MethodInfo generic = method.MakeGenericMethod(c.GetType());

                    Component newC = newObj.AddComponent(c.GetType());

                    generic.Invoke(null, new object[] { c, newC });
                }
            }
        }

        public static void CopyValues<T>(T from, T to)
        {
            var json = JsonUtility.ToJson(from);
            JsonUtility.FromJsonOverwrite(json, to);
        }

        /// <summary>
        /// Return a child transform in the <paramref name="obj"/> transform
        /// </summary>
        /// <param name="name"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static Transform FindChildByName(string name, Transform obj) 
        {
            Transform returnObj;
            if (obj.name == name) 
                return obj;
            foreach (Transform child in obj) 
            {
                returnObj = FindChildByName(name, child);
                if(returnObj)
                    return returnObj;
            }
            return null;
        }
    }
}
