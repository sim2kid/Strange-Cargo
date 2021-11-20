using Creature.Task;
using Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Brain
{
    public interface IUtility
    {
        public float BaseUtility { get; }
        public float[] StatsEffect { get; }
        public ITask RelatedTask { get; }
        public IObject RelatedObject { get; }
    }
}