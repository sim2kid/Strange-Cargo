using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PersistentData.Component;
using PersistentData.Saving;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine.Events;

namespace PersistentData
{
    public class SaveManager : MonoBehaviour
    {
        public UnityEvent OnPreSerialization;
        public UnityEvent OnPreDeserialization;
        public UnityEvent OnPostDeserialization;

        static string SaveLocation;
        [SerializeField]
        bool useEncryption = false;
        Save s;
        
        private void Awake()
        {
            SaveLocation = System.IO.Path.Combine(Application.persistentDataPath, "Saves");
            var others = FindObjectsOfType<SaveManager>();
            if (others != null)
                if (others.Length > 1)
                {
                    Console.LogWarning($"There is already a Save Manager in this scene. Deleting extras.");
                    Destroy(this);
                }
        }

        public Save MakeSave() 
        {
            // Run listeners
            OnPreSerialization.Invoke();

            // Record Prefabbed objects
            PrefabSaveable[] objectsToSave = FindObjectsOfType<PrefabSaveable>();
            var prefabData = new List<PrefabData>();
            foreach (var obj in objectsToSave)
            {
                obj.PreSerialization();
                if(obj.prefabData != null)
                    prefabData.Add(obj.prefabData);
            }
            // Record persistant objects
            PersistentSaveable[] persistentSaveables = FindObjectsOfType<PersistentSaveable>();
            var persisData = new List<ReusedData>();
            foreach (var obj in persistentSaveables)
            {
                obj.PreSerialization();
                if(obj != null)
                    persisData.Add(obj.data);
            }

            // Record creatures
            CreatureSaveable[] creatureSaveables = FindObjectsOfType<CreatureSaveable>();
            var creatureData = new List<GroupData>();
            foreach (var obj in creatureSaveables)
            {
                obj.PreSerialization();
                if (obj != null)
                    creatureData.Add(obj.Data);
            }

            // Make the save
            Save save = new Save()
            {
                GameVersion = Application.version,
                SaveName = "Save Name",
                SaveTime = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Prefabs = prefabData,
                Persistents = persisData,
                Creatures = creatureData
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
            Save save = LoadFromDisk("Save Name");
            Load(save);
            s = save;
            return;
        }

        private void Load(Save save) 
        {
            OnPreDeserialization.Invoke();
            LoadPrefabs(save.Prefabs);
            LoadPersistants(save.Persistents);
            LoadCreatures(save.Creatures);
            OnPostDeserialization.Invoke();
        }

        private void LoadCreatures(List<GroupData> groupData) 
        {
            // Clean Old Creatures
            CreatureSaveable[] currentSaveables = FindObjectsOfType<CreatureSaveable>();
            if (currentSaveables != null)
                foreach (CreatureSaveable current in currentSaveables)
                {
                    // Potentially save data on textures???
                    Destroy(current.gameObject);
                }

            foreach (GroupData data in groupData) 
            {
                CreatureData creature = (CreatureData)data.ExtraData.Find(x => x is CreatureData);
                if (creature == null)
                {
                    Console.LogError($"Could not find creature data. Creature can not be constructed!");
                    continue;
                }
                GameObject creatureObj = Genetics.CreatureGeneration.CreateCreature(creature.dna);
                CreatureSaveable obj = creatureObj.GetComponent<CreatureSaveable>();
                if (obj == null)
                {
                    Console.LogError($"Could not grab the creature saveable for newly generated creature.");
                    continue;
                }
                obj.PreDeserialization();
                obj.Data = data;
                obj.PostDeserialization();
            }
        }

        private void LoadPersistants(List<ReusedData> persistants) 
        {
            List<PersistentSaveable> persistentSaveables = FindObjectsOfType<PersistentSaveable>().ToList();
            foreach (ReusedData data in persistants) 
            {
                PersistentSaveable obj = persistentSaveables.Find(x => x.data.GUID.Equals(data.GUID));
                if (obj == null)
                {
                    Console.LogError($"Could not find persistant of {data.DataType} in the current scene.");
                    Console.LogWarning("We would create it at this time, but that is currently not set up. This is not a forward compatable system.");
                    continue;
                }
                obj.PreDeserialization();
                obj.data = data;
                obj.PostDeserialization();
            }
        }

        private void LoadPrefabs(List<PrefabData> prefabs) 
        {
            // Clean Old Prefabs
            PrefabSaveable[] currentSaveables = FindObjectsOfType<PrefabSaveable>();
            if(currentSaveables != null)
                foreach (PrefabSaveable current in currentSaveables)
                    Destroy(current.gameObject);

            // Place new prefabs
            foreach (PrefabData data in prefabs) 
            {
                var obj = Resources.Load(data.PrefabResourceLocation) as GameObject;
                if (obj == null)
                {
                    if (string.IsNullOrWhiteSpace(data.PrefabResourceLocation))
                        Console.LogError($"Prefabed object did not have a resource location.");
                    else
                        Console.LogWarning($"Can not load prefab from '{data.PrefabResourceLocation}'.");
                    continue;
                }
                GameObject current = Instantiate(obj);
                PrefabSaveable saveable = current.GetComponent<PrefabSaveable>();
                saveable.PreDeserialization();
                saveable.prefabData = data;
                saveable.PostDeserialization();
            }
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

        private byte[] EncryptString(string str) 
        {
            if ((Debug.isDebugBuild && !Application.isEditor) || (Application.isEditor && !useEncryption))
            {
                // No encrypting
                return System.Text.Encoding.UTF8.GetBytes(str);
            }
            else 
            {
                // Yes encrypting
                byte[] bytes = Encrypt(str);
                bytes = bytes.Prepend<byte>((byte)82).Prepend<byte>((byte)110).Prepend<byte>((byte)101).Prepend<byte>((byte)119).Prepend<byte>((byte)79).ToArray();
                return bytes;
            }
        }

        private static string DecryptBytes(byte[] bytes) 
        {
            if (bytes[0].Equals((byte)79) && bytes[1].Equals((byte)119) && bytes[2].Equals((byte)101) && bytes[3].Equals((byte)110) && bytes[4].Equals((byte)82))
            {
                // Actually Encrypted
                bytes = bytes.Skip(5).ToArray();
                string str = Decrypt(bytes);
                return str;
            }
            else 
            {
                // String Encoded
                return System.Text.Encoding.UTF8.GetString(bytes);
            }
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

        private static byte[] Encrypt(string input)
        {
            PasswordDeriveBytes pdb =
              new PasswordDeriveBytes("V$P*9aXhDx@*gK!+",
              new byte[] { 0x13, 0x94, 0x03, 0x22 });
            byte[] encrypted;
            using (Aes aes = new AesManaged())
            {
                aes.Mode = CipherMode.CBC;
                aes.BlockSize = 128;
                aes.FeedbackSize = 128;
                aes.KeySize = 128;
                aes.Padding = PaddingMode.Zeros;
                aes.Key = pdb.GetBytes(aes.KeySize / 8);
                aes.IV = pdb.GetBytes(aes.BlockSize / 8);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms,
                        aes.CreateEncryptor(aes.Key, aes.IV), CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(input);
                        encrypted = ms.ToArray();
                    }
                }
            }
            return encrypted;
        }
        private static string Decrypt(byte[] input)
        {
            PasswordDeriveBytes pdb =
              new PasswordDeriveBytes("V$P*9aXhDx@*gK!+",
              new byte[] { 0x13, 0x94, 0x03, 0x22 });
            string encrypted;
            using (Aes aes = new AesManaged()) 
            {
                aes.Mode = CipherMode.CBC;
                aes.BlockSize = 128;
                aes.FeedbackSize = 128;
                aes.KeySize = 128;
                aes.Padding = PaddingMode.Zeros;
                aes.Key = pdb.GetBytes(aes.KeySize / 8);
                aes.IV = pdb.GetBytes(aes.BlockSize / 8);

                using (MemoryStream ms = new MemoryStream(input)) 
                {
                    using (CryptoStream cs = new CryptoStream(ms,
                        aes.CreateDecryptor(aes.Key, aes.IV), CryptoStreamMode.Read)) 
                    {
                        using (StreamReader reader = new StreamReader(cs))
                            encrypted = reader.ReadToEnd();
                    }
                }
            }
            return encrypted;
        }
    }
}