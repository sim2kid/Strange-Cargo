using PersistentData.Component;
using PersistentData.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placement
{
    public class Itemizable : MonoBehaviour
    {
        [SerializeField]
        private GameObject ItemPrefab;

        private PrefabSaveable prefabSaveable => gameObject.GetComponentInParent<PrefabSaveable>();
        void Start()
        {
            if (prefabSaveable == null)
            {
                Console.LogError($"No prefab saveable parent has been found for the Itemizable component on {name}. " +
                    $"This is required to itemize an object. " +
                    $"The Itemizable component will be destoryed to prevent further conflict.");
                Destroy(this);
            }
            if (ItemPrefab == null) 
            {
                Console.LogWarning($"No item model has been found for {prefabSaveable.name}. As such, a default cube will be used.");
            }
        }

        public GameObject GenerateItem() 
        {
            // Generate Gameobject
            GameObject item = Instantiate(ItemPrefab);
            if (item == null) 
            {
                Console.LogError($"Could not create item for {name}. No prefab gameobject was found.");
                return null;
            }

            // Generate Placeable
            Placeable placeable = item.GetComponent<Placeable>();
            if (placeable == null)
            {
                Console.LogError($"Could not create item for {name}. No placeable script was on the object.");
                Destroy(item);
                return null;
            }

            // Generate Prefab Data
            prefabSaveable.PreSerialization();
            PrefabData data = (PrefabData)prefabSaveable.Data;

            // Hydrate Placeable
            placeable.Hydrate(data);

            // Destroy current object
            Vector3 spawnLoc = FindCenterOfMeshes(prefabSaveable.gameObject);
            Destroy(prefabSaveable.gameObject);

            // Put item object in same spot as the old object
            item.transform.position = spawnLoc;

            return item;
        }

        private Vector3 FindCenterOfMeshes(GameObject root) 
        {
            var Meshes = root.GetComponentsInChildren<Renderer>();
            Vector3 total = Vector3.zero;
            int count = 0;
            foreach (var mesh in Meshes) 
            {
                total += mesh.bounds.center;
                count++;
            }
            total /= count;
            return total;
        }
    }
}