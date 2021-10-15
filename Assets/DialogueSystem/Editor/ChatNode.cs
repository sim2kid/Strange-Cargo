using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace DialogueSystem
{
    public class ChatNode : BasicNode
    {
        public string DialogueText 
        {
            get { return TextFields.TryGetValue("DialogueText", out var tmp) ? tmp : string.Empty; } 
            set { TextFields["DialogueText"] = value; } 
        }
        public string CharacterName
        {
            get { return TextFields.TryGetValue("CharacterName", out var tmp) ? tmp : string.Empty; }
            set { TextFields["CharacterName"] = value; }
        }
        public string Audio
        {
            get { return TextFields.TryGetValue("Audio", out var tmp) ? tmp : string.Empty; }
            set { TextFields["Audio"] = value; }
        }

        public static new BasicNode CreateNode(Vector2 location, string defaultText, string guid)
        {
            NodeData node = new NodeData();
            node.TextFields["DialogueText"] = defaultText;
            node.Position = location;
            node.Type = NodeType.Chat;
            return CreateNode(node, guid);
        }

        public static new BasicNode CreateNode(NodeData data, string guid)
        {
            ChatNode node = new ChatNode();

            // Node Data Info
            node._nodeData = data;
            node.Guid = guid;
            node.title = GenerateTitle("Chat:", node.DialogueText);

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
            var outputPort = GeneratePort(node, Direction.Output, Port.Capacity.Multi);
            outputPort.portName = "Next";
            node.outputContainer.Add(outputPort);

            // Guid Label
            node.extensionContainer.Add(new Label($"{node.Guid}") { name = "guid" });

            // Update Graphics and Position
            node.RefreshExpandedState();
            node.RefreshPorts();
            node.SetPosition(new Rect(node._nodeData.Position, DefaltNodeSize));

            return node;
        }

        public override NodeData SaveNodeData()
        {
            return base.SaveNodeData();
        }
    }
}