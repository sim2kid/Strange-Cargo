using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PersistentData.Saving;

namespace PersistentData
{
    public class SaveManager : MonoBehaviour
    {

        public Save MakeSave() 
        {
            Saveable[] objectsToSave = FindObjectsOfType<Saveable>();
            var saveThis = new List<Saveable>();
            foreach(var obj in saveThis)
                obj.PreSerialization();
            saveThis.AddRange(objectsToSave);
            Save save = new Save() 
            {
                GameVersion = Application.version,
                SaveName = "Save Name",
                SaveTime = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                SavedObjects = saveThis
            };
            return save;
        }

        public void SaveToDisk() 
        {
            Save save = MakeSave();
            string s = Newtonsoft.Json.JsonConvert.SerializeObject(save);
            return;
        }
    }
}