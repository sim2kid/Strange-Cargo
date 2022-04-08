using System;

namespace Creature.Task
{
    public class SubTaskWrapper
    {
        /// <summary>
        /// The subtask that is to be used
        /// </summary>
        public ITask SubTask { get; set; }
        /// <summary>
        /// The callback for when the subtask is finished
        /// </summary>
        public Func<CreatureController, ITask, bool> OnFinishCallback { get; set; }

        public SubTaskWrapper(ITask subTask, Func<CreatureController, ITask, bool> callback) 
        {
            SubTask = subTask;
            OnFinishCallback = callback;
        }
    }
}
