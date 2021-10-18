using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using DialogueSystem;

namespace DialogueSystem
{
    public class DialogueGraph : EditorWindow
    {

        private DialogueGraphView _graphView;
        private Toolbar _toolbar;
        private MiniMap _miniMap;
        private string _fileName = "New Dialogue";

        [MenuItem("Window/Dialogue System/Dialogue Graph", false, 3010)]
        public static void OpenDialogueGraphWindow()
        {
            var window = GetWindow<DialogueGraph>();
            window.titleContent = new GUIContent("Dialogue Graph");
        }

        private void OnEnable()
        {
            ConstructGraph();
            GenerateToolbar();
            GenerateMiniMap();
            GenerateContextMenu();
        }

        private void GenerateContextMenu()
        {

            _graphView.AddManipulator(new ContextualMenuManipulator((ContextualMenuPopulateEvent evt) =>
            {
                evt.menu.AppendAction("Create Dialogue Node", (x) =>
                {
                    _graphView.CreateNode("Dialogue Node", NodeType.Dialogue, _graphView.localMousePosition);
                });
                evt.menu.AppendAction("Create Condition Node", (x) =>
                {
                    _graphView.CreateNode("true", NodeType.Branch, _graphView.localMousePosition);
                });
                evt.menu.AppendAction("Create Event Node", (x) =>
                {
                    _graphView.CreateNode("// TODO //", NodeType.Event, _graphView.localMousePosition);
                });
                evt.menu.AppendAction("Create Variable Node", (x) =>
                {
                    _graphView.CreateNode("// TODO //", NodeType.Variable, _graphView.localMousePosition);
                });
                evt.menu.AppendAction("Create Chat Node", (x) =>
                {
                    _graphView.CreateNode("Chat Node", NodeType.Chat, _graphView.localMousePosition);
                });

                evt.menu.AppendAction("Regenerate Code File", (x) =>
                {
                    generateCode();
                });
            }));
        }

        private void GenerateMiniMap()
        {
            _miniMap = new MiniMap { anchored = true };
            _miniMap.SetPosition(new Rect(10, 30, 200, 140));

            _graphView.Add(_miniMap);
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_graphView);
            rootVisualElement.Remove(_toolbar);
        }

        private void ConstructGraph()
        {
            _graphView = new DialogueGraphView
            {
                name = "Dialogue Graph"
            };
            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);
        }
        private void GenerateToolbar()
        {
            _toolbar = new Toolbar();

            var fileNameTextField = new TextField("File Name:");
            fileNameTextField.SetValueWithoutNotify(_fileName);
            fileNameTextField.MarkDirtyRepaint();
            fileNameTextField.RegisterValueChangedCallback(evt =>
            {
                _fileName = evt.newValue;
            });

            _toolbar.Add(fileNameTextField);

            _toolbar.Add(new Button(() => RequestDataOperation(true)) { text = "Save Data" });
            _toolbar.Add(new Button(() => RequestDataOperation(false)) { text = "Load Data" });
            _toolbar.Add(new Button(() => generateCode()) { text = "Regenerate Code" });

            rootVisualElement.Add(_toolbar);
        }

        [MenuItem("Window/Dialogue System/Regenerate Code", false, 3011)]
        private static void generateCode() 
        {
            DialogueCoder.GenerateCode(DialogueCoder.GrabDialogueContainers());
        }

        private void RequestDataOperation(bool save)
        {
            if (string.IsNullOrEmpty(_fileName))
            {
                EditorUtility.DisplayDialog("Invalid File Name!", "File name can not be blank!", "OK");
                return;
            }

            var saveUtility = GraphSaveUtility.GetInstance(_graphView);
            if (save)
                saveUtility.SaveGraph(_fileName);
            else
                saveUtility.LoadGraph(_fileName);
        }
    }
}