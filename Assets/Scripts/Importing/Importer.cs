using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Importing 
{
    public static class Importer 
    {
        /// <summary>
        /// Grabs and records all files from a parent directory.
        /// <paramref name="parentFolder"/> is the parent folder to look into.
        /// <paramref name="fileSearchPattern"/> is the file search pattern for a Directory.GetFiles function.
        /// <paramref name="db"/> is the Database to add info to. Usually used for recursive searches.
        /// <paramref name="topLevelLocation"/> is the top most level location. It's the Streaming Assets folder by default.
        /// </summary>
        /// <param name="parentFolder"></param>
        /// <param name="fileSearchPattern"></param>
        /// <param name="db"></param>
        /// <param name="topLevelLocation"></param>
        /// <returns></returns>
        public static Database Import(string parentFolder, Database db = null, string topLevelLocation = null, params string[] fileExtensions) 
        {
            if (db == null)
                db = ScriptableObject.CreateInstance<Database>();

            if (string.IsNullOrWhiteSpace(topLevelLocation))
                topLevelLocation = Application.streamingAssetsPath;

            string path = SanitizePath(Path.Combine(topLevelLocation, parentFolder));

            // Prevent running files if there is no folder // 
            if (!Directory.Exists(path))
                return db;

            string[] preFiles = Directory.GetFiles(path);
            List<string> files = new List<string>();
            foreach (string file in preFiles)
                if (fileExtensions.Any(x => file.EndsWith(x)))
                    files.Add(file);
            string[] refrences = Directory.GetFiles(path, "*.ref");
            foreach (string reff in refrences)
            {
                
                string newFile = ResolveRef(reff);
                if (string.IsNullOrWhiteSpace(newFile))
                    continue;
                if (fileExtensions.Any(x => newFile.EndsWith(x)))
                {
                    files.Add(newFile);
                }
            }

            string parent = SanitizePath(parentFolder);
            if (files.Count > 0)
                if (!db.Folders.ContainsKey(parent))
                    db.Folders.Add(parent, new Folder() 
                    {
                        FolderName = parent
                    });

            foreach (string file in files) 
            {
                db.Folders[parent].Files.Add(new File()
                {
                    ParentFolder = parent,
                    FileLocation = SanitizePath(Path.Combine(parent, Path.GetFileName(file)))
                });
            }

            string[] directories = Directory.GetDirectories(path);
            foreach (string dir in directories) 
            {
                string newPath = SanitizePath(Path.Combine(parent, Path.GetFileName(dir)));
                Import(newPath, db, topLevelLocation, fileExtensions);
            }

            return db;
        }

        /// <summary>
        /// Intended to only be run inside of the unity editor.
        /// Will save the <paramref name="database"/> as an asset at the <paramref name="location"/> provided.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="database"></param>
        /// <param name="name"
        public static void SaveDatabase(Database database, string location, string name) 
        {
        #if UNITY_EDITOR

            database.Serialize();

            if (AssetDatabase.IsValidFolder(location))
            {
                AssetDatabase.CreateAsset(database, $"{ForwardSlashPath(Path.Combine(location, name))}.asset");
                AssetDatabase.SaveAssets();
                Debug.Log($"Database \"{name}.asset\" has been saved at \"{location}\".");
            }
            else 
            {
                Debug.LogError($"Could not save Database {name} because \"{location}\" does not exist.");
            }

        #endif
        }

        /// <summary>
        /// Loads a Database from resources at the <paramref name="resourcePath"/> with the file name of <paramref name="fileName"/>
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Database LoadDatabase(string resourcePath, string fileName) 
        {
            Database db = Resources.Load<Database>(SanitizePath(Path.Combine(resourcePath, fileName)));
            if (db == null) 
            {
                Debug.LogError($"Could not find the Database \"{fileName}\" at \"{resourcePath}\". Is everything spelled correctly?");
                return null;
            }
            db.DeSerialize();
            return db;
        }

        private static string ResolveRef(string refPath)
        {
            if (!System.IO.File.Exists(refPath))
                return string.Empty;
            string jsonText = System.IO.File.ReadAllText(refPath);
            JObject jsonObj = (JObject)JToken.Parse(jsonText);
            if (jsonObj.ContainsKey("Path"))
                return (string)jsonObj["Path"];
            return string.Empty;
        }

        private static string SanitizePath(string s)
        {
            return s.Replace('/', '\\');
        }

        private static string ForwardSlashPath(string s) 
        {
            return s.Replace('\\', '/');
        }
    }
}