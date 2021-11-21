using Creature.Stats;
using Creature.Task;
using Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Brain
{
    public class BasicUtility : IUtility
    {
        public float BaseUtility { get; private set; }

        public Needs StatsEffect => Needs.Zero;

        public ITask RelatedTask { get; private set; }

        public IObject RelatedObject => null;

        public BasicUtility(float utility, ITask task) 
        {
            BaseUtility = utility;
            RelatedTask = task;
        }
    }
}