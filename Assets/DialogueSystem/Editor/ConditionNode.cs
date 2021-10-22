using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace DialogueSystem
{
    public class ConditionNode : BasicNode
    {
        public string Condition
        {
            get { return TextFields.TryGetValue("Condition", out var tmp) ? tmp : string.Empty; }
            set { TextFields["Condition"] = value; }
        }

        public static new BasicNode CreateNode(Vector2 location, string defaultText, string guid)
        {
            NodeData node = new NodeData();
            node.TextFields["Condition"] = defaultText;
            node.Position = location;
            node.Type = NodeType.Branch;
            return CreateNode(node, guid);
        }

        public static new BasicNode CreateNode(NodeData data, string guid)
        {
            ConditionNode node = new ConditionNode();

            // Node Data Info
            node._nodeData = data;
            node.Guid = guid;
            node.title = "Condition";

            // Style Sheet
            node.styleSheets.Add(Resources.Load<StyleSheet>("Node"));

            // Text Input Fields //
            var textContainer = new VisualElement
            {
                name = "bottom"
            };

            // Condition:
            textContainer.Add(GenerateTextInput("Condition:", node.Condition, evt =>
            {
                node.Condition = evt.newValue;
            }, "script"));

            node.Add(textContainer);

            // Port Info //
            // Input
            var inputPort = GeneratePort(node, Direction.Input, Port.Capacity.Multi);
            inputPort.portName = "Input";
            node.inputContainer.Add(inputPort);

            // Output
            var passPort = GeneratePort(node, Direction.Output, Port.Capacity.Multi);
            passPort.portName = "Pass";
            node.outputContainer.Add(passPort);

            var failPort = GeneratePort(node, Direction.Output, Port.Capacity.Multi);
            failPort.portName = "Fail";
            node.outputContainer.Add(failPort);

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