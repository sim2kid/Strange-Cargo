using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PersistentData.Component
{
    public class AssetPathEditorWindow : EditorWindow
    {
        public string path = "\\file.json";
        public bool successful { get; private set; } = false;

        public delegate void Action();
        public Action action;

        public static AssetPathEditorWindow Init() 
        {
            AssetPathEditorWindow window = ScriptableObject.CreateInstance<AssetPathEditorWindow>();
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
            window.successful = false;
            window.path = string.Empty;
            window.ShowPopup();
            return window;
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Pick a save path in your Assets folder.", 
                EditorStyles.wordWrappedLabel);
            path = GUILayout.TextField(path);
            if (GUILayout.Button("Submit")) 
            {
                successful = true;
                if(action != null)
                    action.Invoke();
                this.Close();
            }
            if (GUILayout.Button("Cancel"))
            {
                this.Close();
            }
        }
    }
}
