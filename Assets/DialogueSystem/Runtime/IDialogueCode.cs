using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;

namespace DialogueSystem.Code
{
    public interface IDialogueCode
    {
        public delegate void EventDelegate();
        public delegate bool ConditionDelegate();
        public Dictionary<string, EventDelegate> EventFunctions { get; }
        public Dictionary<string, ConditionDelegate> ConditionChecks { get; }
        public Dictionary<string, ConditionDelegate> DialogueChecks { get; }
        public string GetVariable(string variableName);
    }
}