using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Stats
{
    public interface INeedChange
    {
        public Needs NeedChange { get; }
    }
}