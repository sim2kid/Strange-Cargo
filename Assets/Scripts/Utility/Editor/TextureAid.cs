using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Utility;

namespace EditorAid
{
    public class TextureAid : EditorWindow
    {
        private Toolbar _toolbar;

        [MenuItem("Window/Dev Aid/Texture Aid", false, 3110)]
        private static void OpenTextureAid() 
        {
            TextureAid window = EditorWindow.GetWindow<TextureAid>();
            window.titleContent = new GUIContent("Texture Aid");
            window.Show();
        }
        private void OnEnable() 
        {
            GenerateToolbar();
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_toolbar);
        }

        private void GenerateToolbar()
        {
            _toolbar = new Toolbar();

            rootVisualElement.Add(_toolbar);
        }
    }
}