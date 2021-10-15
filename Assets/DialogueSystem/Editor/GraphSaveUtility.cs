using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using DialogueSystem;

namespace DialogueSystem
{
    public class GraphSaveUtility
    {
        private DialogueGraphView _targetGraphView;
        private DialogueContainer _containerCache;

        private List<Edge> Edges => _targetGraphView.edges.ToList();
        private List<BasicNode> Nodes => _targetGraphView.nodes.ToList().Cast<BasicNode>().ToList();
        private List<Port> Ports => _targetGraphView.ports.ToList();

        public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView)
        {
            return new GraphSaveUtility
            {
                _targetGraphView = targetGraphView
            };
        }
        
        /// <summary>
        /// Save the instance of Dialogue Graph View to a file with the name of <paramref name="fileName"/>.
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveGraph(string fileName)
        {
            var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();

            dialogueContainer.DialogueName = fileName;

            // Port Saving

            if (Edges.Any())
            {
                var connectedPorts = Edges.Where(x => x.input.node != null).ToArray();
                for (var i = 0; i < connectedPorts.Length; i++)
                {
                    BasicNode outputNode = connectedPorts[i].output.node as BasicNode;
                    BasicNode inputNode = connectedPorts[i].input.node as BasicNode;
                    var lookingForGUID = connectedPorts[i].output.portName;


                    if (outputNode.Type == NodeType.Dialogue && !outputNode.EntryPoint)
                    {
                        var outputPort = ((DialogueNode)outputNode).outputPorts.Find(x => x.GUID == lookingForGUID);
                        dialogueContainer.NodeLinks.Add(new NodeLinkData
                        {
                            BaseNodeGuid = outputNode.Guid,
                            PortGUID = connectedPorts[i].output.portName,
                            PortName = outputPort.Value,
                            Condition = outputPort.Condition,
                            TargetNodeGuid = inputNode.Guid
                        });
                    }
                    else
                    {
                        dialogueContainer.NodeLinks.Add(new NodeLinkData
                        {
                            BaseNodeGuid = outputNode.Guid,
                            PortName = connectedPorts[i].output.portName,
                            Condition = "",
                            TargetNodeGuid = inputNode.Guid,
                            PortGUID = Guid.NewGuid().ToString()
                        });
                    }
                }
            }

            var unconnectedPorts = Ports.Where(x => x.connected == false && x.direction == Direction.Output).ToArray();
            for (var i = 0; i < unconnectedPorts.Length; i++)
            {
                BasicNode outputNode = unconnectedPorts[i].node as BasicNode;

                if (outputNode.Type == NodeType.Dialogue)
                {
                    var outputPort = ((DialogueNode)outputNode).outputPorts.Find(x => x.GUID == unconnectedPorts[i].portName);
                    dialogueContainer.NodeLinks.Add(new NodeLinkData
                    {
                        BaseNodeGuid = outputNode.Guid,
                        PortName = outputPort.Value,
                        Condition = outputPort.Condition,
                        TargetNodeGuid = "",
                        PortGUID = unconnectedPorts[i].portName
                    });
                }
            }

            // Node Saving. All of this will be defined by the node itself

            dialogueContainer.EntryPointGUID = Nodes.Find(x => x.EntryPoint).Guid;

            foreach (var node in Nodes.Where(node => !node.EntryPoint))
                dialogueContainer.Nodes.Add(node.SaveNodeData());

            // Creating Asset (And asset folder)

            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");

            if (!AssetDatabase.IsValidFolder("Assets/Resources/DialogueTrees"))
                AssetDatabase.CreateFolder("Resources", "DialogueTrees");

            AssetDatabase.CreateAsset(dialogueContainer, $"Assets/Resources/DialogueTrees/{fileName}.asset");
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// Load an instance of Dialogue Graph View from a file with the name of <paramref name="fileName"/>.
        /// </summary>
        /// <param name="fileName"></param>
        public void LoadGraph(string fileName)
        {
            _containerCache = Resources.Load<DialogueContainer>("DialogueTrees/" + fileName);

            if (_containerCache == null)
            {
                EditorUtility.DisplayDialog("File Not Found", "Target dialogue graph file does not exist!", "OK");
                return;
            }

            ClearGraph();
            CreateNodes();
            ConnectNodes();
        }

        private void ConnectNodes()
        {
            for (var i = 0; i < Nodes.Count; i++)
            {
                var connections = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == Nodes[i].Guid && !string.IsNullOrEmpty(x.TargetNodeGuid)).ToList();
                for (var j = 0; j < connections.Count; j++)
                {
                    List<Port> outputPorts;

                    BasicNode baseNode = Nodes.First(x => x.Guid == connections[j].BaseNodeGuid);

                    if (baseNode.Type != NodeType.Dialogue)
                        outputPorts = Ports.Where(x => x.portName == connections[j].PortName).ToList();
                    else
                        outputPorts = Ports.Where(x => x.portName == connections[j].PortGUID).ToList();


                    var targetNodeGuid = connections[j].TargetNodeGuid;
                    var targetNode = Nodes.First(x => x.Guid == targetNodeGuid);

                    Port basePort = outputPorts.Where(x => ((BasicNode)x.node).Guid == connections[j].BaseNodeGuid).First();
                    Port targetPort = (Port)targetNode.inputContainer[0];
                    LinkNodes(basePort, targetPort);

                }
            }
        }

        private void LinkNodes(Port output, Port input)
        {
            var tempEdge = new Edge
            {
                output = output,
                input = input
            };

            tempEdge?.input.Connect(tempEdge);
            tempEdge?.output.Connect(tempEdge);

            _targetGraphView.Add(tempEdge);
        }

        private void CreateNodes()
        {
            foreach (var node in _containerCache.Nodes)
            {
                node.DeSerialize();
                if (node.Type == NodeType.Dialogue)
                {
                    // For dialogue nodes, we need to look at the links, so we pull that one out seperatly
                    var tempNode = (DialogueNode)_targetGraphView.CreateNode(node);
                    var nodePorts = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == node.Guid).ToList();
                    nodePorts.ForEach((x) =>
                    {
                        if (tempNode.outputPorts.Find(y => y.GUID == x.PortGUID) == null)
                            DialogueNode.AddChoicePort(tempNode, x.PortName, x.Condition, x.PortGUID);
                    });
                }
                else 
                {
                    // Everything else is okay with just being added as is.
                    _targetGraphView.CreateNode(node);
                }
                
            }
        }

        private void ClearGraph()
        {
            var entryPoint = Nodes.Find(x => x.EntryPoint);
            entryPoint.Guid = _containerCache.EntryPointGUID;

            foreach (var node in Nodes)
            {
                if (node.EntryPoint) continue;
                Edges.Where(x => x.input.node == node).ToList().ForEach(edge => _targetGraphView.RemoveElement(edge));

                _targetGraphView.RemoveElement(node);
            }
        }
    }
}