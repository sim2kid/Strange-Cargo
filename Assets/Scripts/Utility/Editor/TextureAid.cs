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
        private string[] hexStrings = new string[3];
        private string savePathString = "C:\\temp\\";
        private string saveNameString = "output.png";

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
            if (output == null) 
            {
                runConversion();
            }
        }

        private void SetUp()
        {
            if (colors[0] == null) 
            {
                colors[0] = Color.clear;
                colors[1] = Color.clear;
                colors[2] = Color.clear;
            }
            if (hexStrings[0] == null) 
            {
                hexStrings[0] = string.Empty;
                hexStrings[1] = string.Empty;
                hexStrings[2] = string.Empty;
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

            TextField[] hexs = new TextField[3];

            for (int i = 0; i < 3; i++) 
            {
                Label label = new Label($"Color {i+1}: ");
                UI.Add(label);
                hexs[i] = new TextField();
                hexs[i].value = hexStrings[i];
                if (ColorUtility.TryParseHtmlString($"#{hexs[i].value}", out Color color))
                {
                    color.a = 1;
                    colors[i] = color;
                }
                else 
                {
                    hexs[i].value = ColorUtility.ToHtmlStringRGB(colors[i]);
                }
                hexStrings[i] = hexs[i].value;
                UI.Add(hexs[i]);
            }

            Button confirm = new Button();
            confirm.text = "Save Colors";
            UI.Add(confirm);
            confirm.clicked += () =>
            {
                for (int i = 0; i < 3; i++)
                {
                    hexStrings[i] = hexs[i].value;
                }
                ReDraw();
            };

            if (output != null)
            {
                Label pathLabel = new Label("Save Path:");
                TextField savePath = new TextField();
                savePath.value = savePathString;
                Label nameLabel = new Label("File Name:");
                TextField fileName = new TextField();
                fileName.value = saveNameString;
                Button saveButton = new Button();
                saveButton.text = "Save File";
                saveButton.clicked += () => 
                {
                    savePathString = savePath.value;
                    saveNameString = fileName.value;
                    SaveImage($"{savePathString}\\{saveNameString}", output);
                };
                UI.Add(pathLabel);
                UI.Add(savePath);
                UI.Add(nameLabel);
                UI.Add(fileName);
                UI.Add(saveButton);
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
                foward.RegisterCallback<ChangeEvent<bool>>((ChangeEvent<bool> evt) => 
                {
                    baseOperation = foward.value;
                    ReDraw();
                });
                _toolbar.Add(foward);
                Button redraw = new Button();
                redraw.text = "Convert";
                redraw.clicked += () =>
                {
                    runConversion();
                };
                _toolbar.Add(redraw);
            }


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
                    runConversion();
                    ReDraw();
                }
                catch (Exception e)
                {
                    fileLocation.value = path = string.Empty;
                    working = null;
                    output = null;
                }
            };

            rootVisualElement.Add(_toolbar);
        }

        private void runConversion() 
        {
            if (working == null)
                return;
            if (baseOperation)
            {
                output = TextureConversions.GenerateBaseTexture(working, colors);
            }
            else 
            {
                output = TextureConversions.ConvertTexture(working, colors);
            }
            SaveImage("C:\\Users\\2simm\\Downloads\\image.png", output);
        }

        private void SaveImage(string path, Texture2D texture) 
        {
            byte[] imageData = ImageConversion.EncodeToPNG(texture);
            File.WriteAllBytes(path, imageData);
        }

        private Texture2D LoadImage(string path) 
        {
            Texture2D texture = new Texture2D(1,1);
            ImageConversion.LoadImage(texture, File.ReadAllBytes(path));
            return texture;
        }
    }
}