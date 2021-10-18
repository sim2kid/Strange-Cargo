using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace DialogueSystem
{
    public class VariableNode : BasicNode
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
            node.Type = NodeType.Variable;
            return CreateNode(node, guid);
        }

        public static new BasicNode CreateNode(NodeData data, string guid)
        {
            VariableNode node = new VariableNode();

            // Node Data Info
            node._nodeData = data;
            node.Guid = guid;
            node.title = GenerateTitle("Variables");

            // Style Sheet
            node.styleSheets.Add(Resources.Load<StyleSheet>("Node"));

            // Text Input Fields //
            var textContainer = new VisualElement
            {
                name = "bottom"
            };
            // Code:
            textContainer.Add(GenerateTextInput("", node.Code, evt =>
            {
                node.Code = evt.newValue;
            }, "script"));

            node.Add(textContainer);


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