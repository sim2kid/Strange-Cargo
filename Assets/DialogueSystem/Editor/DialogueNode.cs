using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace DialogueSystem
{
    public class DialogueNode : ChatNode
    {
        private DialogueGraphView.RemovePortDelegate RemovePort;

        public static new BasicNode CreateNode(Vector2 location, string defaultText, string guid)
        {
            throw new System.Exception("Missing Remove Port Delegate");
        }
        public static new BasicNode CreateNode(NodeData data, string guid)
        {
            throw new System.Exception("Missing Remove Port Delegate");
        }
        public static BasicNode CreateNode(Vector2 location, string defaultText, string guid, DialogueGraphView.RemovePortDelegate remove)
        {
            NodeData node = new NodeData();
            node.TextFields["DialogueText"] = defaultText;
            node.Position = location;
            node.Type = NodeType.Dialogue;
            return CreateNode(node, guid, remove);
        }
        public static BasicNode CreateNode(NodeData data, string guid, DialogueGraphView.RemovePortDelegate remove)
        {
            DialogueNode node = new DialogueNode();

            // Node Data Info
            node._nodeData = data;
            node.Guid = guid;
            node.title = GenerateTitle("Chat:", node.DialogueText);
            node.RemovePort = remove;

            // Style Sheet
            node.styleSheets.Add(Resources.Load<StyleSheet>("Node"));

            // Text Input Fields //
            var textContainer = new VisualElement
            {
                name = "bottom"
            };
            // Character Name:
            textContainer.Add(GenerateTextInput("Character Name:", node.CharacterName, evt =>
            {
                node.CharacterName = evt.newValue;
            }));

            // Dialogue Text
            textContainer.Add(GenerateTextInput("Dialogue Text:", node.DialogueText, evt =>
            {
                node.DialogueText = evt.newValue;
                node.title = GenerateTitle("Chat:", node.DialogueText);
            }));

            // Audio:
            textContainer.Add(GenerateTextInput("Audio File:", node.Audio, evt =>
            {
                node.Audio = evt.newValue;
            }));

            node.Add(textContainer);

            // Port Info //
            // Input Ports
            var inputPort = GeneratePort(node, Direction.Input, Port.Capacity.Multi);
            inputPort.portName = "Input";
            node.inputContainer.Add(inputPort);

            // Output Ports

            var button = new Button(() =>
            {
                AddChoicePort(node, "");
            });
            button.text = "+";
            node.titleContainer.Add(button);

            // Guid Label
            node.extensionContainer.Add(new Label($"{node.Guid}") { name = "guid" });

            // Update Graphics and Position
            node.RefreshExpandedState();
            node.RefreshPorts();
            node.SetPosition(new Rect(node._nodeData.Position, DefaltNodeSize));

            return node;
        }

        public static void AddChoicePort(DialogueNode dialogueNode, string overriddenPortName = "", string conditions = "", string overriddenGUID = "")
        {
            var generatedPort = GeneratePort(dialogueNode, Direction.Output, Port.Capacity.Multi);
            var outputPortCount = dialogueNode.outputContainer.Query("connector").ToList().Count;
            var outputPort = new OutputPort
            {
                NodeGUID = dialogueNode.Guid,
                GUID = (string.IsNullOrEmpty(overriddenGUID) ? System.Guid.NewGuid().ToString() : overriddenGUID),
                Condition = conditions,
                Value = ""
            };
            generatedPort.portName = outputPort.GUID;


            var oldLabel = generatedPort.contentContainer.Q<Label>("type");
            generatedPort.contentContainer.Remove(oldLabel);

            var choicePortName = string.IsNullOrEmpty(overriddenPortName) ? $"Choice {outputPortCount}" : overriddenPortName;

            outputPort.Value = choicePortName;

            var textField = new TextField
            {
                name = string.Empty,
                value = choicePortName,
                isDelayed = true,
                multiline = true
            };
            textField.RegisterValueChangedCallback(evt =>
            {
                outputPort.Value = evt.newValue;
            });

            var conditionField = new TextField
            {
                name = "script",
                value = conditions,
                multiline = true
            };
            conditionField.RegisterValueChangedCallback(evt =>
            {
                outputPort.Condition = evt.newValue;
            });

            var mainContainer = new VisualElement
            {
                name = "mainContainer"
            };


            generatedPort.contentContainer.Add(new Label("  "));
            mainContainer.Add(textField);
            mainContainer.Add(new Label("Condition:"));
            mainContainer.Add(conditionField);
            generatedPort.contentContainer.Add(mainContainer);

            var deleteButton = new Button(() => dialogueNode.RemovePort(dialogueNode, generatedPort))
            {
                text = "-"
            };
            generatedPort.contentContainer.Add(deleteButton);

            dialogueNode.outputPorts.Add(outputPort);

            dialogueNode.outputContainer.Add(generatedPort);
            dialogueNode.RefreshExpandedState();
            dialogueNode.RefreshPorts();
        }

    }
}