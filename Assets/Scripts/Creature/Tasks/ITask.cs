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
        /// Will run the task.
        /// </summary>
        /// <param name="caller">The creature calling this task</param>
        /// <returns>Returns the task itself for chaining</returns>
        public ITask RunTask(CreatureController caller);

        /// <summary>
        /// The update loop
        /// </summary>
        /// <param name="caller">Creature calling the update</param>
        /// <returns>Returns itself for chaining</returns>
        public void Update(CreatureController caller);

        /// <summary>
        /// Run to clean up a task's resources.
        /// </summary>
        /// <param name="update"></param>
        public void EndTask(CreatureController caller);

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