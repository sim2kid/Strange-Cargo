using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PersistentData.Component;
using PersistentData.Saving;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace PersistentData
{
    public class SaveManager : MonoBehaviour
    {
        static string SaveLocation;
        Save s;
        private void Awake()
        {
            SaveLocation = System.IO.Path.Combine(Application.persistentDataPath, "Saves");
        }

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

        public void DemoSave() 
        {
            Save save = MakeSave();
            SaveToDisk(save);
        }

        public void DemoLoad() 
        {
            s = LoadFromDisk("Save Name");
            return;
        }

        private Save LoadFromDisk(string saveName) 
        {
            string loc = SanitizePath(Path.Combine(SaveLocation, RemoveIllegalCharacters(saveName) + ".save"));

            if (!Directory.Exists(SaveLocation))
                Directory.CreateDirectory(SaveLocation);
            if (!File.Exists(loc)) 
            {
                Console.LogWarning($"Could not find save '{saveName}'. Is it spelled correctly?");
                return new Save() { SaveTime = -1 };
            }

            byte[] saveBytes = File.ReadAllBytes(loc);
            string saveJson = DecryptBytes(saveBytes);

            Save save = JsonConvert.DeserializeObject<Save>(saveJson);
            return save;
        }

        private void SaveToDisk(Save save) 
        {
            string loc = SanitizePath(Path.Combine(SaveLocation, RemoveIllegalCharacters(save.SaveName) + ".save"));
            string saveJson = JsonConvert.SerializeObject(save);

            if(!Directory.Exists(SaveLocation))
                Directory.CreateDirectory(SaveLocation);

            File.WriteAllBytes(loc, EncryptString(saveJson));

            return;
        }

        private static byte[] EncryptString(string str) 
        {
            return System.Text.Encoding.UTF8.GetBytes(str);
        }

        private static string DecryptBytes(byte[] bytes) 
        {
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        private static bool HasIlligalCharacters(string s) 
        {
            return !s.Equals(RemoveIllegalCharacters(s));
        }

        private static string RemoveIllegalCharacters(string s) 
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(s, "");
        }

        private static string SanitizePath(string s)
        {
            return s.Replace('/', '\\');
        }
    }
}