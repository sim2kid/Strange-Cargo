using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Creature.Task
{
    public interface ITask
    {
        /// <summary>
        /// Returns true when the task is complete
        /// </summary>
        public bool IsDone { get; }
        /// <summary>
        /// Returns false until the task is started
        /// </summary>
        public bool IsStarted { get; }
        /// <summary>
        /// Will invoke when IsDone turns true
        /// </summary>
        public UnityEvent OnTaskFinished { get; }
        /// <summary>
        /// Will run the task. Provide the <paramref name="caller"/> and an <paramref name="update"/> method to give tools to the task.
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="update"></param>
        /// <returns>Returns the task itself for chaining</returns>
        public ITask RunTask(CreatureController caller, UnityEvent update);
        /// <summary>
        /// Run to clean up a task's resources.
        /// </summary>
        /// <param name="update"></param>
        public void EndTask(UnityEvent update);

        /// <summary>
        /// The amount of satisfaction returned after preforming a task
        /// </summary>
        public float Satisfaction { get; }

        /// <summary>
        /// The hook for a satisfaction event
        /// </summary>
        /// <returns></returns>
        public void SatisfactionHook(System.Func<float> func);
    }
}