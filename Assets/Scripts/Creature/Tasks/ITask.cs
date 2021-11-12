using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Creature.Task
{
    public interface ITask
    {
        public bool IsDone { get; }
        public bool IsStarted { get; }

        public UnityEvent OnTaskFinished { get; }

        public ITask RunTask(CreatureController caller, UnityEvent update);
        public void EndTask(UnityEvent update);

        
    }
}