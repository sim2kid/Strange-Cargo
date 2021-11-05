using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Importing;

namespace Utility
{
    [InitializeOnLoad]
    public class DatabaseBuilder
    {
        static DatabaseBuilder()
        {
            UpdateSoundDatabase();
        }

        public static void UpdateSoundDatabase() 
        {
            Database db = Importer.Import("", "*.mp3", null, "Assets/Resources/Audio");
            Importer.SaveDatabase(db, "Assets/Resources/Data/Database", "Audio");
        }
    }
}