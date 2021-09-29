using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using Utility;

namespace EditorAid
{
    public class TextureAid : EditorWindow
    {
        private Toolbar _toolbar;
        private VisualElement UI;
        private bool showAbout { get { return working == null; } }
        private string path = string.Empty;
        private Texture2D working;
        private Texture2D output;
        private bool showOriginal = true;
        private bool baseOperation = false;
        private Color[] colors = new Color[3];

        [MenuItem("Window/Dev Aid/Texture Aid", false, 3110)]
        private static void OpenTextureAid() 
        {
            TextureAid window = EditorWindow.GetWindow<TextureAid>();
            window.titleContent = new GUIContent("Texture Aid");
            window.Show();
        }
        private void OnEnable() 
        {
            SetUp();
            GenerateToolbar();
            GenerateUI();
        }

        private void SetUp()
        {
            if (colors[0] == null) 
            {
                colors[0] = Color.clear;
                colors[1] = Color.clear;
                colors[2] = Color.clear;
            }
        }

        private void GenerateUI()
        {
            UI = new VisualElement();
            VisualElement AboutSection = new VisualElement();
            // Texture Aid Text
            TextElement title = new TextElement();
            title.text = "This modual will help you swap between actual textures and randomizable textures.";
            AboutSection.Add(title);

            TextElement GrabFile = new TextElement();
            GrabFile.text = "Pick a file to work with on the toolbar to begin.";
            AboutSection.Add(GrabFile);

            if (showAbout)
            {
                UI.Add(AboutSection);
            }
            else
            {
                Image sample = new Image();
                sample.image = working;
                sample.scaleMode = ScaleMode.ScaleToFit;
                Image changed = new Image();
                changed.scaleMode = ScaleMode.ScaleToFit;

                if (output != null)
                    changed.image = output;

                if (!showOriginal && output != null)
                    UI.Add(changed);
                else
                    UI.Add(sample);
            }


            rootVisualElement.Add(UI);
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_toolbar);
            rootVisualElement.Remove(UI);
        }

        private void ReDraw() 
        {
            OnDisable();
            OnEnable();
        }

        private void GenerateToolbar()
        {
            _toolbar = new Toolbar();

            TextElement getFileText = new TextElement();
            getFileText.text = " File Location: ";
            _toolbar.Add(getFileText);

            TextField fileLocation = new TextField();
            fileLocation.value = path;
            _toolbar.Add(fileLocation);

            ToolbarButton fileExplorer = new ToolbarButton();
            fileExplorer.text = "Load File";
            _toolbar.Add(fileExplorer);
            fileExplorer.clicked += () =>
            {
                string fileURI = fileLocation.value;
                path = fileURI;
                try
                {
                    working = LoadImage(fileURI);
                    ReDraw();
                }
                catch (Exception e) 
                {
                    fileLocation.value = path = string.Empty;
                    working = null;
                    output = null;
                }
            };

            if (!showAbout) {
                ToolbarButton originalOrNot = new ToolbarButton();
                if (showOriginal)
                    originalOrNot.text = "Show Modified";
                else
                    originalOrNot.text = "Show Original";
                originalOrNot.clicked += () =>
                {
                    showOriginal = !showOriginal;
                    ReDraw();
                };
                _toolbar.Add(originalOrNot);

                Toggle foward = new Toggle();
                foward.value = baseOperation;
                if(baseOperation)
                    foward.text = "Colors to RGB";
                else
                    foward.text = "RGB to Colors";
                _toolbar.Add(foward);
            }


            rootVisualElement.Add(_toolbar);
        }

        private void runConversion() 
        {
            if(baseOperation)

        }

        private Texture2D LoadImage(string path) 
        {
            Texture2D texture = new Texture2D(1,1);
            ImageConversion.LoadImage(texture, File.ReadAllBytes(path));
            return texture;
        }
    }
}