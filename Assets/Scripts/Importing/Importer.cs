using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
        public static Database Import(string parentFolder, string fileSearchPattern = null, Database db = null, string topLevelLocation = null) 
        {
            if (db == null)
                db = new Database();

            if (string.IsNullOrWhiteSpace(topLevelLocation))
                topLevelLocation = Application.streamingAssetsPath;

            string path = SanitizePath(Path.Combine(topLevelLocation, parentFolder));
            string[] files;
            if (string.IsNullOrEmpty(fileSearchPattern))
                files = Directory.GetFiles(path);
            else
                files = Directory.GetFiles(path, fileSearchPattern);
            string parent = SanitizePath(parentFolder);
            if (files.Length > 0)
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
                    FileLocation = file
                });
            }

            string[] directories = Directory.GetDirectories(path);
            foreach (string dir in directories) 
            {
                string newPath = SanitizePath(Path.Combine(parent, Path.GetFileName(dir)));
                Import(newPath, fileSearchPattern, db, topLevelLocation);
            }

            return db;
        }

        private static string SanitizePath(string s)
        {
            return s.Replace('/', '\\');
        }
    }
}