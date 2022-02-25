using PersistentData;
using PersistentData.Saving;
using Placement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Spawn(PrefabData prefab) 
    {
        GameObject obj = SaveManager.LoadPrefab(prefab);
        var furnature = obj.GetComponentInChildren<Itemizable>();
        if (furnature != null) 
        {
            obj = furnature.GenerateItem();
        }

        obj.transform.position = this.transform.position;
    }
}
