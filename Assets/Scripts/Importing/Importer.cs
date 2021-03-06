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
        /// </summary>
        /// <param name="parentFolder">The parent folder to look into. This is used for recursive searches.</param>
        /// <param name="db">The Database to add info to. Usually used for recursive searches.</param>
        /// <param name="topLevelLocation">The top most level location. It's the Streaming Assets folder by default.</param>
        /// <param name="fileExtensions">All the file extensions to look for</param>
        /// <returns>Database full of records.</returns>
        public static Database Import(string parentFolder, Database db = null, string topLevelLocation = null, params string[] fileExtensions) 
        {
            if (db == null)
                db = ScriptableObject.CreateInstance<Database>();

            if (string.IsNullOrWhiteSpace(topLevelLocation))
                topLevelLocation = Application.streamingAssetsPath;

            // Full file location startinf from the toplevel + parent folder loc
            string path = SanitizePath(Path.Combine(topLevelLocation, parentFolder));

            // Prevent running files if there is no folder // 
            if (!Directory.Exists(path))
                return db;

            // Pulls files that match the ending pattern
            string[] preFiles = Directory.GetFiles(path); // absolute paths
            List<string> files = new List<string>();
            foreach (string file in preFiles)
                if (fileExtensions.Any(x => file.ToLower().EndsWith(x.ToLower())))
                    files.Add(file);

            // Creates the parent folder in the database to store each file.
            string parent = SanitizePath(parentFolder);
            if (files.Count > 0)
                if (!db.Folders.ContainsKey(parent))
                    db.Folders.Add(parent, new Folder()
                    {
                        FolderName = parent
                    });

            // Resolves Refrences
            string[] refrences = Directory.GetFiles(path, "*.ref");
            foreach (string reff in refrences)
            {
                // Returns path related to topLevelLocation
                string relitive = ResolveRef(reff);
                // Get absolute location
                string absolutePath = SanitizePath(Path.Combine(topLevelLocation, relitive));

                if (string.IsNullOrWhiteSpace(absolutePath))
                    continue;

                string refNameNoExtension = Path.GetFileNameWithoutExtension(reff);

                // Check if the file exists
                if (!System.IO.File.Exists(absolutePath))
                {
                    continue;
                }

                // if the file exists and has a valid ending, add it to folder directly
                if (fileExtensions.Any(x => absolutePath.ToLower().EndsWith(x.ToLower())))
                {
                    db.Folders[parent].Files.Add(new File() 
                    {
                        ParentFolder = parent,
                        FileLocation = relitive,
                        FileName = refNameNoExtension + Path.GetExtension(absolutePath)
                    });
                    files.Add(absolutePath);
                }
            }

            // store each file in the parent folder in the database
            foreach (string file in files) 
            {
                string location = SanitizePath(Path.Combine(parent, Path.GetFileName(file)));
                db.Folders[parent].Files.Add(new File()
                {
                    ParentFolder = parent,
                    FileLocation = location,
                    FileName = Path.GetFileName(location)
                });
            }

            // Recursivly look through the database/folders
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

            try
            {
                database.Serialize();

                if (database == null || string.IsNullOrEmpty(location) || string.IsNullOrEmpty(name))
                    return;

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

            }
            catch 
            {
                Debug.LogWarning("Could not save the database at this time.");
            }
            #endif
            if (!Application.isEditor) 
            {
                Debug.LogError($"Can't save a database durring gameplay!! DB: {location}/{name}");
            }
        }

        /// <summary>
        /// Loads a Database from resources at the <paramref name="resourcePath"/> with the file name of <paramref name="fileName"/>
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Database LoadDatabase(string resourcePath, string fileName) 
        {
            string dbLoc = ForwardSlashPath(Path.Combine(resourcePath, fileName));
            Database db = Resources.Load<Database>(dbLoc);
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