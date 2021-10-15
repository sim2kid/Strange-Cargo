using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;
using System;

namespace DialogueSystem
{
    [Flags]
    public enum NodeType
    {
        Entry = 1 << 1,
        Dialogue = 1 << 2,
        Chat = 1 << 3,
        Variable = 1 << 4,
        Branch = 1 << 5,
        Condition = 1 << 5,
        Event = 1 << 6,
        Exit = 1 << 7,
        None = 0
    }
}