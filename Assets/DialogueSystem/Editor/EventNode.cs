using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace DialogueSystem
{
    public class EventNode : BasicNode
    {
        public string Code
        {
            get { return TextFields.TryGetValue("Code", out var tmp) ? tmp : string.Empty; }
            set { TextFields["Code"] = value; }
        }
        public static new BasicNode CreateNode(Vector2 location, string defaultText, string guid)
        {
            NodeData node = new NodeData();
            node.TextFields["Code"] = defaultText;
            node.Position = location;
            node.Type = NodeType.Event;
            return CreateNode(node, guid);
        }

        public static new BasicNode CreateNode(NodeData data, string guid)
        {
            EventNode node = new EventNode();

            // Node Data Info
            node._nodeData = data;
            node.Guid = guid;
            node.title = GenerateTitle("Event");

            // Style Sheet
            node.styleSheets.Add(Resources.Load<StyleSheet>("Node"));

            // Text Input Fields //
            var textContainer = new VisualElement
            {
                name = "bottom"
            };
            // Code:
            textContainer.Add(GenerateTextInput("Code:", node.Code, evt =>
            {
                node.Code = evt.newValue;
            }, "script"));

            node.Add(textContainer);

            // Input
            var inputPort = GeneratePort(node, Direction.Input, Port.Capacity.Multi);
            inputPort.portName = "Input";
            node.inputContainer.Add(inputPort);

            // Guid Label
            node.extensionContainer.Add(new Label($"{node.Guid}") { name = "guid" });

            // Update Graphics and Position
            node.RefreshExpandedState();
            node.RefreshPorts();
            node.SetPosition(new Rect(node._nodeData.Position, DefaltNodeSize));

            return node;
        }
    }
}