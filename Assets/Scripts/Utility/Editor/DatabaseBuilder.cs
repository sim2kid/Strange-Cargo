using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using Importing;

namespace Utility
{
    public class DatabaseBuilder : IPreprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }
        public void OnPreprocessBuild(BuildReport report)
        {
            UpdateSoundDatabase();
        }


        public void UpdateSoundDatabase() 
        {
            Database db = Importer.Import("Audio", "*.mp3", null, "Assets/Resources");
            Importer.SaveDatabase(db, "Assets/Resources/Data/Database", "Audio");
        }
    }
}