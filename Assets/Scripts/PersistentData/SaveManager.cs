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

        public UnityEvent OnLoad;

        static string SaveLocation;
        [SerializeField]
        bool useEncryption = false;
        SaveMeta CurrentSave;

        private void Awake()
        {
            SaveLocation = System.IO.Path.Combine(Application.persistentDataPath, "saves");
            var others = FindObjectsOfType<SaveManager>();
            if (others != null)
                if (others.Length > 1)
                {
                    Console.LogWarning($"There is already a Save Manager in this scene. Deleting extras.");
                    Destroy(this);
                }
            string guid = System.Guid.NewGuid().ToString();
            CurrentSave = new SaveMeta()
            {
                GameVersion = Application.version,
                SaveName = "New Save",
                SaveGuid = guid,
                SaveTime = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };
        }

        public void StartNewSave(string name) 
        {
            string guid = System.Guid.NewGuid().ToString();
            CurrentSave = new SaveMeta()
            {
                GameVersion = Application.version,
                SaveName = name,
                SaveGuid = guid,
                SaveTime = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };
            Save save = GetCleanSave();
            save.Metadata = CurrentSave;
            Load(save);
            Invoke("Save", 2);
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
                if (obj.prefabData != null)
                    prefabData.Add(obj.prefabData);
            }
            // Record persistant objects
            PersistentSaveable[] persistentSaveables = FindObjectsOfType<PersistentSaveable>();
            var persisData = new List<ReusedData>();
            foreach (var obj in persistentSaveables)
            {
                obj.PreSerialization();
                if (obj != null)
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

            SaveMeta meta = new SaveMeta()
            {
                GameVersion = Application.version,
                SaveName = CurrentSave.SaveName,
                SaveGuid = CurrentSave.SaveGuid,
                SaveTime = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            // Make the save
            Save save = new Save()
            {
                Metadata = meta,
                Prefabs = prefabData,
                Persistents = persisData,
                Creatures = creatureData
            };
            return save;
        }

        public void Save()
        {
            Save save = MakeSave();
            SaveToDisk(save);
        }

        public void LoadSave(string guid) 
        {
            Save save = LoadFromDisk(guid);
            Load(save);
            CurrentSave = save.Metadata;
            return;
        }

        private void Load(Save save)
        {
            OnLoad.Invoke();
            OnPreDeserialization.Invoke();
            LoadPrefabs(save.Prefabs);
            LoadPersistants(save.Persistents);
            LoadCreatures(save.Creatures);
            OnPostDeserialization.Invoke();
        }

        private void DeleteDirectory(string path) 
        {
            foreach (string s in Directory.GetDirectories(path)) 
            {
                DeleteDirectory(s);
            }
            foreach (string s in Directory.GetFiles(path))
            {
                File.Delete(s);
            }
            Directory.Delete(path, true);
        }

        public void DeleteSave(string saveGuid) 
        {
            string saveFolder = SanitizePath(Path.Combine(SaveLocation, RemoveIllegalCharacters(saveGuid)));
            if (Directory.Exists(saveFolder)) 
            {
                DeleteDirectory(saveFolder);
            }
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

            if (groupData == null)
                return;

            foreach (GroupData data in groupData)
            {
                CreatureData creature = (CreatureData)data.ExtraData.Find(x => x is CreatureData);
                if (string.IsNullOrWhiteSpace(creature.GUID))
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
            if (persistants == null)
                return;

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
            if (currentSaveables != null)
                foreach (PrefabSaveable current in currentSaveables)
                    Destroy(current.gameObject);

            if (prefabs == null)
                return;

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

        public List<SaveMeta> GetSaveList() 
        {
            List<SaveMeta> metas = new List<SaveMeta>();
            if (string.IsNullOrEmpty(SaveLocation))
                return metas;
            string[] saves = Directory.GetDirectories(SaveLocation);
            foreach (var save in saves)
            {
                string dataLoc = SanitizePath(Path.Combine(save, $"save.dat"));
                string metaLoc = SanitizePath(Path.Combine(save, $"meta.dat"));
                if (File.Exists(dataLoc) && File.Exists(metaLoc)) 
                {
                    byte[] metaBytes = File.ReadAllBytes(metaLoc);
                    string metaJson = DecryptBytes(metaBytes);
                    SaveMeta meta = JsonConvert.DeserializeObject<SaveMeta>(metaJson);
                    if(!string.IsNullOrEmpty(meta.SaveName))
                        metas.Add(meta);
                }
            }
            return metas;
        }

        private Save GetCleanSave() 
        {
            return new Save();
        }

        private Save LoadFromDisk(string saveGuid)
        {
            string saveFolder = SanitizePath(Path.Combine(SaveLocation, RemoveIllegalCharacters(saveGuid)));
            string dataLoc = SanitizePath(Path.Combine(saveFolder, $"save.dat"));

            if (!Directory.Exists(SaveLocation))
                Directory.CreateDirectory(SaveLocation);
            if (!Directory.Exists(saveFolder))
            {
                Console.LogWarning($"Could not find load '{saveGuid}'. Is it spelled correctly?");
                return new Save() { Metadata = new SaveMeta() { SaveTime = -1 } };
            }
            if (!File.Exists(dataLoc))
            {
                Console.LogWarning($"Could not find load '{saveGuid}'. Is it spelled correctly?");
                return new Save() { Metadata = new SaveMeta() { SaveTime = -1 } };
            }

            byte[] saveBytes = File.ReadAllBytes(dataLoc);
            string saveJson = DecryptBytes(saveBytes);

            Save save = JsonConvert.DeserializeObject<Save>(saveJson);
            return save;
        }

        private void SaveToDisk(Save save)
        {
            string saveFolder = SanitizePath(Path.Combine(SaveLocation, RemoveIllegalCharacters(save.Metadata.SaveGuid)));

            if (!Directory.Exists(SaveLocation))
                Directory.CreateDirectory(SaveLocation);

            if (!Directory.Exists(saveFolder))
                Directory.CreateDirectory(saveFolder);

            string dataLoc = SanitizePath(Path.Combine(saveFolder, $"save.dat"));
            string metaLoc = SanitizePath(Path.Combine(saveFolder, $"meta.dat"));
            string saveJson = JsonConvert.SerializeObject(save);
            string metaJson = JsonConvert.SerializeObject(save.Metadata);

            File.WriteAllBytes(dataLoc, EncryptString(saveJson));
            File.WriteAllBytes(metaLoc, StringToBytes(metaJson));

            return;
        }

        private static byte[] StringToBytes(string str)
        {
            return System.Text.Encoding.UTF8.GetBytes(str);
        }

        private byte[] EncryptString(string str)
        {
            if ((Debug.isDebugBuild && !Application.isEditor) || (Application.isEditor && !useEncryption))
            {
                // No encrypting
                return StringToBytes(str);
            }
            else
            {
                // Yes encrypting
                byte[] bytes = Encrypt(str);
                bytes = PrependArray(bytes, CreatePattern(secrets[Random.Range(0,secrets.Length)]));
                return bytes;
            }
        }

        private static byte[] PrependArray(byte[] other, params byte[] addons)
        {
            for (int i = addons.Length - 1; i >= 0; i--)
            {
                other = other.Prepend<byte>(addons[i]).ToArray();
            }
            return other;
        }

        private static byte[] CreatePattern(string s)
        {
            return StringToBytes(s);
        }

        private static bool StartsWithPattern(byte[] other, byte[] pattern)
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                if (i < other.Length)
                {
                    if (pattern[i] != other[i])
                        return false;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        private static byte[] StartsWithAnyPatterns(byte[] other, params string[] patterns)
        {
            if (patterns == null)
                return null;
            foreach (string pattern in patterns)
            {
                byte[] barr = CreatePattern(pattern);
                if (StartsWithPattern(other, barr))
                    return barr;
            }
            return null;
        }

        private static string[] secrets = new string[] 
        {
            "OwenR",
            "Granny Is Super Old",
            "I see you snooping",
            "up up down down left right left right B A start",
            "Fishboi is constantly floating between a state of immortality and suffocating due to lack of water",
            "Just don't check the basement",
            "Granny is a communist",
            "Strange-Cargo: Halfway Home",
            "Strange Cargo 2: Sons of Liberty",
            "Strange Cargo 4: Gloos of the Patriots",
            "Granny Doesn't Call 911",
            "Granny once had a basement before the police found it",
            "Granny has been kidnapped by ninjas. Are you a bad enough dude to rescue Granny?",
            "Please hire me, they won't pay me here"
        };

        private static string DecryptBytes(byte[] bytes) 
        {
            byte[] pattern = StartsWithAnyPatterns(bytes, secrets);
            if (pattern != null)
            {
                // Actually Encrypted
                bytes = bytes.Skip(pattern.Length).ToArray();
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