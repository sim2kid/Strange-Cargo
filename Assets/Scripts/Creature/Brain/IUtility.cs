using Creature.Stats;
using Creature.Task;
using DataType;
using Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Brain
{
    public interface IUtility
    {
        public ValueRange BaseUtility { get; }
        public Needs StatsEffect { get; }
        public ITask RelatedTask { get; }
        public IObject RelatedObject { get; }
    }
}