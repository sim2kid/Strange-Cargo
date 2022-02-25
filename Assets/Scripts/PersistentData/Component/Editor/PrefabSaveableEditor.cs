using PersistentData.Saving;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace PersistentData.Component
{
    [CustomEditor(typeof(PrefabSaveable))]
    [CanEditMultipleObjects]
    public class PrefabSaveableEditor : Editor
    {
        SerializedProperty prefabData;
        AssetPathEditorWindow getPath;
        bool saveGuid = false;

        void OnEnable() 
        {
            prefabData = serializedObject.FindProperty("prefabData");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(prefabData);
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.LabelField("Save Guid:");
            saveGuid = EditorGUILayout.Toggle(saveGuid);
            if (GUILayout.Button("Save As Json")) 
            {
                if (!Application.isPlaying)
                {
                    Debug.LogWarning("Game must be running to save an item to disk!");
                }
                else
                {
                    OpenWindow();
                }
            }
        }

        private void OpenWindow() 
        {
            getPath = AssetPathEditorWindow.Init();
            getPath.path = "\\Resources\\Shop\\Generic\\file.json";
            getPath.action = () => SaveFile(getPath.path);
        }

        public void SaveFile(string location) 
        {
            if (string.IsNullOrEmpty(location)) 
            {
                Debug.LogWarning("Could not save file. Location is invalid.");
                return;
            }
            PrefabSaveable ps = (PrefabSaveable)serializedObject.targetObject;
            ps.PreSerialization();
            PrefabData data = (PrefabData)ps.Data;
            if (!saveGuid) 
            {
                data.GUID = null;
            }
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            File.WriteAllText(Application.dataPath + location, json);
            AssetDatabase.Refresh();
        }
    }
}
