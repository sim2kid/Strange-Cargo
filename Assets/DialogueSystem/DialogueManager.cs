using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;
using DialogueSystem.Code;
using System;
using System.Text.RegularExpressions;
using System.Linq;

namespace DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        [Tooltip("Enter the name of the dialogue you want to use. This is the same name used in the Dialogue Graph.")]
        [SerializeField]
        private string DialogueName = string.Empty;

        private DialogueContainer ActiveDialogue = null; // Our dialogue tree to track
        private IDialogueCode dialogueCode = null; // The tree's dialogue code
        private NodeData currentNode;

        /// <summary>
        /// Returns the dialogue text of the current node
        /// </summary>
        public string DialogueText => getDialogueText();
        /// <summary>
        /// Returns the character text of the current node
        /// </summary>
        public string Character => getCharacter();
        /// <summary>
        /// Returns the audio file text of the current node
        /// </summary>
        public string AudioFile => getAudioFile();
        /// <summary>
        /// Returns the dialogue options of the current node (if avaliable). It's in the format of ( Option Text, Option Guid )
        /// </summary>
        public Dictionary<string, string> DialogueOptions => getDialogueOptions();
        /// <summary>
        /// Returns true if in the middle of an active conversation. It will return false at the end of a conversation.
        /// </summary>
        public bool InConversation { get; private set; }

        private void OnEnable()
        {
            DialogueName = DialogueName.Trim();
            if (!string.IsNullOrWhiteSpace(DialogueName))
            {
                ActiveDialogue = Resources.Load<DialogueContainer>($"DialogueTrees/{DialogueName}");
                if (ActiveDialogue == null) 
                {
                    Debug.LogWarning($"Dialogue, \"{DialogueName}\", could not be found. Are you sure that you've entered your name correctly?");
                }
            }
            InConversation = false;
            currentNode = null;
            StartConversation();
        }

        /// <summary>
        /// This will enable a conversation while preserving the variables from last time it was converced.
        /// </summary>
        public void StartConversation() 
        {
            if (ActiveDialogue != null)
            {
                if(dialogueCode == null)
                    dialogueCode = DialogueCodeUtility.GetDialogueCode(ActiveDialogue.DialogueName);
                // Find the first node
                Next(ActiveDialogue.EntryPointGUID);
                // Mark that we're in a convo
                InConversation = true;
            }
        }
        
        /// <summary>
        /// This will reset the dialogue to it's begining state. (Includes reseting variables as well!!)
        /// </summary>
        public void Reset()
        {
            OnEnable();
        }


        /// <summary>
        /// Sets the working dialogue tree. This will overwrite the current one!
        /// </summary>
        /// <param name="newDialogue"></param>
        public void SetDialogue(DialogueContainer newDialogue) 
        {
            ActiveDialogue = newDialogue;
            OnEnable();
        }

        /// <summary>
        /// Gets ready for the next dialogue piece from the branch specified by the <paramref name="optionGUID"/>.
        /// </summary>
        /// <param name="optionGUID"></param>
        public void Next(string optionGUID) 
        {
            if (currentNode == null) 
            {
                // If the node is blank, we'll grab the start port's output guid and use that
                optionGUID = GetStartPort(optionGUID);
                // Steps through and gets the first node
                currentNode = stepThroughNodes(optionGUID);
                return;
            }

            if (currentNode.Type == NodeType.Dialogue)
                currentNode = stepThroughNodes(optionGUID);
            else
                Debug.LogWarning("DialogueManager.Next(string guid) should only be used on a Dialogue Node!");
        }

        /// <summary>
        /// Gets ready for the next dialogue piece 
        /// </summary>
        public void Next() 
        {
            if (currentNode.Type == NodeType.Chat)
            {
                NodeLinkData NodeLink = ActiveDialogue.NodeLinks.Find(x => x.BaseNodeGuid == currentNode.Guid);
                if (NodeLink == null)
                {
                    endConversation();
                    return;
                }
                string portGuid = NodeLink.PortGUID;
                currentNode = stepThroughNodes(portGuid);
            }
            else
            {
                Debug.LogWarning("DialogueManager.Next should only be used on a Chat Node!");
            }
        }

        /// <summary>
        /// Use this method to grab any text field not predefined. Pass the text field's name in <paramref name="fieldName"/>
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public string GetField(string fieldName)
        {
            if (!InConversation) return null;
            return replaceVariables(GetTextField(currentNode, fieldName));
        }

        /// <summary>
        /// Sets the In Conversation bool to false
        /// </summary>
        private void endConversation() 
        {
            InConversation = false;
        }

        /// <summary>
        /// Steps through the nodes and runs code. The currentNode is set after this.
        /// </summary>
        /// <param name="startOutputPortGUID"></param>
        private NodeData stepThroughNodes(string startOutputPortGUID) 
        {
            NodeData newNode = null;
            List<NodeData> nextNodes = getNextNodes(startOutputPortGUID);
            nextNodes.ForEach(x => 
            {
                switch (x.Type) 
                {
                    case NodeType.Branch:
                        newNode = runBranchCondition(x.Guid);
                        break;
                    case NodeType.Chat:
                        newNode = x;
                        break;
                    case NodeType.Dialogue:
                        newNode = x;
                        break;
                    case NodeType.Event:
                        runEventNode(x);
                        break;
                    case NodeType.Exit:
                        endConversation();
                        break;
                }
            });

            if (newNode == null)
            {
                // end condition
                endConversation();
            }
            else 
            {
                newNode.DeSerialize();
            }

            return newNode;
        }

        /// <summary>
        /// Check if a condition passed or not and runs the related nodes
        /// </summary>
        /// <param name="nodeGUID"></param>
        private NodeData runBranchCondition(string nodeGUID) 
        {
            Dictionary<string, IDialogueCode.ConditionDelegate> branchCondition = dialogueCode.ConditionChecks;
            IDialogueCode.ConditionDelegate conditionCheck;
            branchCondition.TryGetValue(GenerateFunctionName(ActiveDialogue.DialogueName, nodeGUID), out conditionCheck);
            if (conditionCheck == null)
                return null;
            if (conditionCheck())
            {
                // Pass
                return stepThroughNodes(ActiveDialogue.NodeLinks.Find(x => x.PortName == "Pass" && x.BaseNodeGuid == nodeGUID).PortGUID);
            }
            else 
            {
                // Fail
                return stepThroughNodes(ActiveDialogue.NodeLinks.Find(x => x.PortName == "Fail" && x.BaseNodeGuid == nodeGUID).PortGUID);
            }
        }

        /// <summary>
        /// Take a <see cref="NodeLinkData">Port's</see> GUID and returns a list of connected <see cref="NodeData">Nodes</see>.
        /// </summary>
        /// <param name="outputGuid"></param>
        /// <returns></returns>
        private List<NodeData> getNextNodes(string outputGuid) 
        {
            // Get the GUIDs of the targeted nodes
            List<string> guids = new List<string>();
            NodeLinkData basePort = ActiveDialogue.NodeLinks.Find(x => x.PortGUID.Equals(outputGuid));
            NodeData currentNode = ActiveDialogue.Nodes.Find(y => y.Guid.Equals(basePort.BaseNodeGuid));
            if (currentNode == null) 
            {
                currentNode = new NodeData()
                {
                    Type = NodeType.Entry,
                    Guid = basePort.BaseNodeGuid
                };
            }

            if (currentNode.Type == NodeType.Dialogue)
                ActiveDialogue.NodeLinks.FindAll(x => x.PortGUID.Equals(outputGuid)).ForEach(x => guids.Add(x.TargetNodeGuid));
            else
                ActiveDialogue.NodeLinks.FindAll(x => (x.BaseNodeGuid.Equals(currentNode.Guid) && x.PortName.Equals(basePort.PortName))).ForEach(x => guids.Add(x.TargetNodeGuid));

            // Convert the GUIDs into NodeData objects
            List<NodeData> outputNodes = new List<NodeData>();
            guids.ForEach(guid => 
            {
                ActiveDialogue.Nodes.FindAll(x => x.Guid.Equals(guid)).ForEach(x => outputNodes.Add(x));
            });
            return outputNodes;
        }

        /// <summary>
        /// Checks a <see cref="DialogueNodeData">Dialogue Node</see> and runs each condition for the choices.
        /// </summary>
        /// <param name="dialogueNode"></param>
        /// <returns>If the conditions pass, they are returned as Dictionary ( Option Text, Option Guid )</Option></returns>
        private Dictionary<string, string> formDialogueChoices(NodeData dialogueNode) 
        {
            // Find ports in node where the basenode guid's match. Make sure they're not in the list already
            Dictionary<string, string> dialogueOptions = new Dictionary<string, string>();
            ActiveDialogue.NodeLinks.FindAll(x => x.BaseNodeGuid == dialogueNode.Guid).ForEach(x => 
            {
                if(!dialogueOptions.ContainsValue(x.PortGUID))
                    dialogueOptions.Add(x.PortName, x.PortGUID);
            });

            // Check the conditions for the ports to run
            Dictionary<string, string> toReturnOptions = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> option in dialogueOptions)
            {
                if (checkCondition(dialogueNode.Guid, option.Value)) 
                {
                    toReturnOptions.Add(replaceVariables(option.Key), option.Value);
                }
            }

            return toReturnOptions;
        }

        /// <summary>
        /// Runs the condition check in <see cref="GeneratedDialogueCode"/>.
        /// </summary>
        /// <param name="baseNodeGUID"></param>
        /// <param name="portGUID"></param>
        /// <returns></returns>
        private bool checkCondition(string baseNodeGUID, string portGUID) 
        {
            Dictionary<string, IDialogueCode.ConditionDelegate> portConditions = dialogueCode.DialogueChecks;
            IDialogueCode.ConditionDelegate conditionCheck;
            portConditions.TryGetValue(GenerateFunctionName(ActiveDialogue.DialogueName, baseNodeGUID, portGUID), out conditionCheck);
            return conditionCheck();
        }

        /// <summary>
        /// Takes an <see cref="EventNodeData"/> and runs the associated code in <see cref="GeneratedDialogueCode"/>
        /// </summary>
        /// <param name="eventNode"></param>
        private void runEventNode(NodeData eventNode) 
        {
            if (eventNode.Type != NodeType.Event) return;

            Dictionary<string, IDialogueCode.EventDelegate> events = dialogueCode.EventFunctions;
            IDialogueCode.EventDelegate eventFunction;
            events.TryGetValue(GenerateFunctionName(ActiveDialogue.DialogueName, eventNode.Guid), out eventFunction);
            eventFunction();
        }

        /// <summary>
        /// Generates the name of a function inside of the <see cref="GeneratedDialogueCode"/> class.
        /// <paramref name="portGuid"/> is optional but is needed for Conditions on <see cref="DialogueNodeData">Dialogue Nodes</see>.
        /// </summary>
        /// <param name="dialogueName"></param>
        /// <param name="nodeGuid"></param>
        /// <param name="portGuid"></param>
        /// <returns></returns>
        private static string GenerateFunctionName(string dialogueName, string nodeGuid, string portGuid = "") 
        {
            return DialogueCodeUtility.GenerateFunctionName(dialogueName, nodeGuid, portGuid);
        }

        /// <summary>
        /// Returns the output guid of the start node
        /// </summary>
        /// <param name="startNodeGuid"></param>
        /// <returns></returns>
        private string GetStartPort(string startNodeGuid) 
        {
            string outputGuid = ActiveDialogue.NodeLinks.Find(x => x.BaseNodeGuid == startNodeGuid).PortGUID;
            return outputGuid;
        }

        private Dictionary<string, string> getDialogueOptions()
        {
            if (!InConversation) return null;
            if (currentNode.Type == NodeType.Dialogue)
            {
                return formDialogueChoices(currentNode);
            }
            else
            {
                return null;
            }
        }
        private string getCharacter()
        {
            if (!InConversation) return null;
            if (currentNode.Type == NodeType.Chat || currentNode.Type == NodeType.Dialogue)
            {
                return replaceVariables(GetTextField(currentNode, "CharacterName"));
            }
            else
            {
                return null;
            }
        }
        private string getDialogueText()
        {
            if (!InConversation) return null;
            if (currentNode.Type == NodeType.Chat || currentNode.Type == NodeType.Dialogue)
            {
                return replaceVariables(GetTextField(currentNode, "DialogueText"));
            }
            else 
            {
                return null;
            }
        }
        private string getAudioFile() 
        {
            if (!InConversation) return null;
            if (currentNode.Type == NodeType.Chat || currentNode.Type == NodeType.Dialogue)
            {
                return replaceVariables(GetTextField(currentNode, "Audio"));
            }
            else
            {
                return null;
            }
        }

        private string replaceVariables(string text) 
        {
            MatchCollection matches = Regex.Matches(text, @"(?<=\$\{).*?(?=\})");
            List<string> matchList = matches.Cast<Match>().Select(match => match.Value).ToList();
            foreach (string vari in matchList) 
            {
                try
                {
                    string variableValue = dialogueCode.GetVariable(vari);
                    string oldValue = "${" + vari + "}";
                    text = text.Replace(oldValue, variableValue);
                } 
                catch (Exception e)
                {
                    Debug.LogError(e);
                    //Debug.LogWarning($"The variable, {vari}, is not present in your Dialogue Tree, {ActiveDialogue.DialogueName}." +
                    //    $" This was from the node with the GUID of {currentNode.Guid}.");
                }
            }
            return text;
        }

        private static string GetTextField(NodeData node, string field)
        {
            
            string text = DialogueCodeUtility.GetTextField(node, field);
            return string.IsNullOrEmpty(text) ? null : text;
        }
    }
}