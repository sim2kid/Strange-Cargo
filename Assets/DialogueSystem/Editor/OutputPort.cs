using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Linq;
using DialogueSystem;

namespace DialogueSystem
{
    [Serializable]
    public class OutputPort
    {
        public string NodeGUID;
        public string GUID;
        public string Value;
        public string Condition;
    }
}
