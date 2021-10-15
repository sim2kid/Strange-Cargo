using System;
using UnityEngine;
using DialogueSystem;

namespace DialogueSystem
{
    [Serializable]
    public class NodeLinkData
    {
        /// <summary>
        /// The GUID of the Parent Node
        /// </summary>
        public string BaseNodeGuid;
        /// <summary>
        /// The Text of the Node
        /// </summary>
        public string PortName;
        /// <summary>
        /// The Output Port's GUID
        /// </summary>
        public string PortGUID;
        /// <summary>
        /// The Condition of the Node (Should be addressable in the <see cref="GeneratedDialogueCode"/> class)
        /// </summary>
        public string Condition;
        /// <summary>
        /// The GUID of the node that this port targets
        /// </summary>
        public string TargetNodeGuid;
    }
}