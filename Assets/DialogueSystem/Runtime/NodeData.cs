using DialogueSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [Serializable]
    public class NodeData
    {
        public string Guid;
        public Vector2 Position;
        public NodeType Type;

        // These will store our Dictionary Stuff
        [SerializeField]
        private List<string> _textKeys = new List<string>();
        [SerializeField]
        private List<string> _textValues = new List<string>();

        /// <summary>
        /// Before storing this Object, you need to Serialize it!
        /// </summary>
        public void Serialize() 
        {
            _textKeys = new List<string>();
            _textValues = new List<string>();
            foreach (KeyValuePair<string, string> keyValue in TextFields)
            {
                _textKeys.Add(keyValue.Key);
                _textValues.Add(keyValue.Value);
            }
        }

        /// <summary>
        /// Before loading this object, you need to DeSerialize it!
        /// </summary>
        public void DeSerialize()
        {
            TextFields = new Dictionary<string, string>();
            for (int i = 0; i < _textKeys.Count; i++) 
            {
                TextFields.Add(_textKeys[i], _textValues[i]);
            }
        }

        public Dictionary<string, string> TextFields = new Dictionary<string, string>();
    }
}