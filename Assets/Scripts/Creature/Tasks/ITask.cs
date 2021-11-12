using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Task
{
    public interface ITask
    {
        public bool IsDone { get; }
        public bool IsStarted { get; }

        public void RunTask(CreatureController caller);
    }
}