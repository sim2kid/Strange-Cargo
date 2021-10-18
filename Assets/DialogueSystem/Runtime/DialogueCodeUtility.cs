using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DialogueSystem.Code
{
    public static class DialogueCodeUtility
    {
        public static string GenerateFunctionName(string dialogueName, string nodeGuid, string portGuid = "")
        {
            return $"{SanitizeName(dialogueName)}_{nodeGuid.Replace("-", "")}{(string.IsNullOrEmpty(portGuid) ? string.Empty : ("_" + portGuid.Replace("-", "")))}";
        }

        public static string SanitizeName(string name)
        {
            name = name.Trim();
            // Replace non alphanumerics with an '_'
            name = Regex.Replace(name, @"([^0-9a-zA-Z_])+", "_");
            // Make sure the begining isn't a number or underscore
            name = Regex.Replace(name, @"^([0-9_])+", string.Empty);
            return name;
        }

        public static string GenerateClassName(string name) 
        {
            name = name.Trim() + "_DialogueCode";
            return SanitizeName(name);
        }
        public static string GetTextField(NodeData node, string field)
        {
            node.TextFields.TryGetValue(field, out string output);
            return (string.IsNullOrEmpty(output) ? string.Empty : output);
        }

        public static IDialogueCode GetDialogueCode(string dialogueName) 
        {
            string className = $"DialogueSystem.Code.{GenerateClassName(dialogueName)}";
            Assembly asm = typeof(IDialogueCode).Assembly;
            Type type = asm.GetType(className);
            if (type != null)
                return (IDialogueCode)Activator.CreateInstance(type);
            return null;
        }
    }
}