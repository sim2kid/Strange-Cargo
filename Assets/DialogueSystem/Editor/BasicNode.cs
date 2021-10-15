using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using DialogueSystem;
using System;
using UnityEngine.UIElements;
using UnityEngine;

namespace DialogueSystem
{
    public class BasicNode : Node, IGraphNode
    {
        protected static readonly Vector2 DefaltNodeSize = new Vector2(150, 200);
        protected NodeData _nodeData;

        public string Guid { get { return _nodeData.Guid; } set { _nodeData.Guid = value; } }
        public NodeType Type { get { return _nodeData.Type; } set { _nodeData.Type = value; } }
        public Dictionary<string, string> TextFields { get { return _nodeData.TextFields; } set { _nodeData.TextFields = value; } }

        public bool EntryPoint { get; set; }
        public List<OutputPort> outputPorts { get; set; }
    

        public BasicNode()
        {
            _nodeData = new NodeData();
            EntryPoint = false;
            outputPorts = new List<OutputPort>();
            Type = NodeType.None;
        }

        public static BasicNode CreateNode(Vector2 location, string defaultText, string guid)
        {
            NodeData node = new NodeData();
            node.Position = location;
            return CreateNode(node, guid);
        }
        public static BasicNode CreateNode(NodeData data, string guid)
        {
            throw new System.Exception("Can Not Make a \"Basic Node\" Type of Node");
        }

        public virtual NodeData SaveNodeData()
        {
            _nodeData.Position = this.GetPosition().position;
            _nodeData.Serialize();
            return _nodeData;
        }

        // ~ Node Drawing Tools ~ //
        public static Port GeneratePort(BasicNode node, Direction portDirection, Port.Capacity capacity)
        {
            return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
        }

        protected static VisualElement GenerateTextInput(string label, string startValue, EventCallback<ChangeEvent<string>> onChange, string name = "") 
        {
            // Create a container to put everything in
            VisualElement visualElement = new VisualElement { name = label };

            if (!string.IsNullOrEmpty(label))
            {
                // Make a label for our input and add it
                visualElement.Add(new Label(label));
            }

            // Here's our actual input
            TextField text = new TextField(string.Empty) { name = name };
            text.multiline = true;
            text.SetValueWithoutNotify(startValue);

            // Event Callback 
            text.RegisterValueChangedCallback(onChange);

            // Add the textField to the Visual Element
            visualElement.Add(text);

            return visualElement;
        }
        protected static string GenerateTitle(string label, string extra = "") 
        {
            return $"{label} {limit(extra,20)}";
        }
        
        private static string limit(string str, int length)
        {
            return (str.Length <= length ? str : $"{str.Substring(0, length - 3)}...");
        }

    }
}
