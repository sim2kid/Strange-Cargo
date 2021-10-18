using System.Collections;
using System.Collections.Generic;
using System.IO;
using DialogueSystem;
using DialogueSystem.Code;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem
{
    public static class DialogueCoder
    {
        // Code to make code
        public static List<DialogueContainer> GrabDialogueContainers()
        {
            DialogueContainer[] containers = Resources.LoadAll<DialogueContainer>("DialogueTrees");
            List<DialogueContainer> listOfContainers = new List<DialogueContainer>();
            foreach (DialogueContainer c in containers)
                listOfContainers.Add(c);
            return listOfContainers;
        }

        public static void GenerateCode(List<DialogueContainer> containers)
        {
            CleanDirectory();
            foreach (DialogueContainer container in containers)
            {
                string setUp = $"{Tab(3)}// Setup //\n";
                string variables = $"{Tab(2)}// Variables //\n";
                string eventFunctions = $"{Tab(2)}// Event Functions //\n";
                string conditionChecks = $"{Tab(2)}// Condition Checks //\n";
                string dialogueChecks = $"{Tab(2)}// Dialogue Checks //\n";
                string treeName = DialogueCodeUtility.GenerateClassName(container.DialogueName);

                foreach (NodeData node in container.Nodes)
                {
                    node.DeSerialize();
                    string functionName = string.Empty;
                    switch (node.Type)
                    {
                        case NodeType.Variable:
                            variables += $"{Tab(2)}// Variable(s) From Node: {node.Guid} //\n" +
                                $"{GetTextField(node,"Code")}\n";
                            break;

                        case NodeType.Event:
                            functionName = DialogueCodeUtility.GenerateFunctionName(container.DialogueName, node.Guid);

                            eventFunctions += $"{Tab(2)}// Event From Node: {node.Guid} //\n" +
                                $"{Tab(2)}public void {functionName}() {{\n" +
                                $"{GetTextField(node, "Code")}\n" +
                                $"{Tab(2)}}}\n";

                            setUp += $"{Tab(3)}eventFunctions.Add(\"{functionName}\",{functionName});\n";
                            break;

                        case NodeType.Branch:
                            functionName = DialogueCodeUtility.GenerateFunctionName(container.DialogueName, node.Guid);

                            conditionChecks += $"{Tab(2)}// Condition From Node: {node.Guid} //\n" +
                                $"{Tab(2)}public bool {functionName}() {{\n" +
                                $"{Tab(3)}return ({(string.IsNullOrEmpty(GetTextField(node, "Condition")) ? $"true" : GetTextField(node, "Condition"))});\n" +
                                $"{Tab(2)}}}\n";

                            setUp += $"{Tab(3)}conditionChecks.Add(\"{functionName}\",{functionName});\n";
                            break;

                        case NodeType.Dialogue:
                            List<string> ListedPorts = new List<string>(); // Record of already displayed ports
                            foreach (NodeLinkData port in container.NodeLinks)
                            {       // Make sure the link's base node matches our current node       // Make sure that the port hasn't been used yet
                                if (port.BaseNodeGuid == node.Guid && !ListedPorts.Contains(port.PortGUID.ToString()))
                                {
                                    functionName = DialogueCodeUtility.GenerateFunctionName(container.DialogueName, node.Guid, port.PortGUID);

                                    dialogueChecks += $"{Tab(2)}// From Node: {node.Guid} //\n{Tab(2)}// Choice: {port.PortName} - {port.PortGUID} //\n";
                                    dialogueChecks += $"{Tab(2)}public bool {functionName}()\n{Tab(2)}{{\n{Tab(3)}return (";
                                    dialogueChecks += (string.IsNullOrEmpty(port.Condition.Trim()) ? "true" : port.Condition);
                                    dialogueChecks += $");\n{Tab(2)}}}\n";

                                    setUp += $"{Tab(3)}dialogueChecks.Add(\"{functionName}\",{functionName});\n";
                                    ListedPorts.Add(port.PortGUID.ToString());
                                }
                            }
                            break;
                    }
                }
                CodeBuilder(setUp, variables, dialogueChecks, conditionChecks, eventFunctions, treeName);
            }
        }

        private static string GetTextField(NodeData node, string field) 
        {
            return DialogueCodeUtility.GetTextField(node, field);
        }

        private static void CodeBuilder(string setUp, string variables, string dialogueChecks, string conditionNodesChecks, string eventNodeFunctions, string treeName)
        {
            string precode1 = @"using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using DialogueSystem;
using System.Reflection;

namespace DialogueSystem.Code
{
    public class ";

            string precode2 = @" : IDialogueCode
    {
        private Dictionary<string, IDialogueCode.EventDelegate> eventFunctions = new Dictionary<string, IDialogueCode.EventDelegate>();
        private Dictionary<string, IDialogueCode.ConditionDelegate> conditionChecks = new Dictionary<string, IDialogueCode.ConditionDelegate>();
        private Dictionary<string, IDialogueCode.ConditionDelegate> dialogueChecks = new Dictionary<string, IDialogueCode.ConditionDelegate>();
        public Dictionary<string, IDialogueCode.EventDelegate> EventFunctions => eventFunctions;
        public Dictionary<string, IDialogueCode.ConditionDelegate> ConditionChecks => conditionChecks;
        public Dictionary<string, IDialogueCode.ConditionDelegate> DialogueChecks => dialogueChecks;
        public string GetVariable(string variableName) {
            return this.GetType().GetField(variableName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(this).ToString(); 
        }
        public ";
            string precode3 = @"() 
        {
            Start();
        }
";
            string postcode = @"    }
}";
            string toWrite = $"{precode1}{treeName}{precode2}{treeName}{precode3}\n\n{variables}\n\n{Tab(2)}public void Start()\n{Tab(2)}{{\n{setUp}\n{Tab(2)}}}" +
                $"\n\n{dialogueChecks}\n\n{conditionNodesChecks}\n\n{eventNodeFunctions}\n{postcode}";

            WriteString(toWrite, treeName);
        }

        private static string Tab(int amount = 1)
        {
            string back = string.Empty;
            for (int i = 0; i < amount; i++)
                back += "    ";
            return back;
        }

        private static string SanitizeName(string name)
        {
            return DialogueCodeUtility.SanitizeName(name);
        }
        private static void WriteString(string code, string file)
        {
            string path = $"Assets/DialogueSystem/Runtime/GeneratedCode/{file}.cs";

            // Clean inconsistant new line characters
            code = Regex.Replace(code, @"\r\n?|\n", System.Environment.NewLine);

            if (!AssetDatabase.IsValidFolder("Assets/DialogueSystem/Runtime/GeneratedCode"))
                AssetDatabase.CreateFolder("Assets/DialogueSystem/Runtime", "GeneratedCode");

            // Write the code to a CS file
            StreamWriter writer = new StreamWriter(path, false);
            writer.WriteLine(code);
            writer.Close();

            // Tell unity that there's a new file present
            AssetDatabase.Refresh();
        }

        private static void CleanDirectory() 
        {
            if (!AssetDatabase.IsValidFolder("Assets/DialogueSystem/Runtime/GeneratedCode"))
                AssetDatabase.CreateFolder("Runtime", "GeneratedCode");
            string[] CodeFolder = { "Assets/DialogueSystem/Runtime/GeneratedCode" };
            foreach (var codeFile in AssetDatabase.FindAssets(string.Empty, CodeFolder)) 
            {
                var path = AssetDatabase.GUIDToAssetPath(codeFile);
                AssetDatabase.DeleteAsset(path);
            }
        }
    }

}