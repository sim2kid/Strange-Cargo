using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PersistentData.Component;
using PersistentData.Saving;

namespace PersistentData
{
    public class SaveManager : MonoBehaviour
    {
        string s;
        public Save MakeSave() 
        {
            PrefabSaveable[] objectsToSave = FindObjectsOfType<PrefabSaveable>();
            var saveThis = new List<PrefabData>();
            foreach (var obj in objectsToSave)
            {
                obj.PreSerialization();
                saveThis.Add(obj._prefabData);
            }
            Save save = new Save() 
            {
                GameVersion = Application.version,
                SaveName = "Save Name",
                SaveTime = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Prefabs = saveThis
            };
            return save;
        }

        public void SaveToDisk() 
        {
            Save save = MakeSave();
            s = Newtonsoft.Json.JsonConvert.SerializeObject(save);
            return;
        }
    }
}