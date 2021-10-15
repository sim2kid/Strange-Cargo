using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DialogueSystem;

namespace DialogueSystem
{
    [Serializable]
    public class DialogueContainer : ScriptableObject
    {
        public string DialogueName = "";
        public string EntryPointGUID = "";
        public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();
        public List<NodeData> Nodes = new List<NodeData>();
    }
}