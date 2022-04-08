using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Creature.Task
{
    public class Wander : ITask
    {
        public bool IsDone { get; private set; }

        public bool IsStarted { get; private set; }

        public UnityEvent OnTaskFinished { get; private set; }

        private float distance;
        private GoHere movementSubtask;
        private Wait waitTask;
        private DataType.ValueRange maxTime;

        private CreatureController _caller;
        private UnityEvent _update;

        public float Satisfaction { get; private set; }
        public void SatisfactionHook(System.Func<float> func)
        {
        }

        public void EndTask(CreatureController caller)
        {
            IsStarted = false;
        }

        public ITask RunTask(CreatureController caller)
        {
            IsStarted = true;
            IsDone = false;
            movementSubtask = new GoHere(caller.transform.position + (Random.insideUnitSphere * distance), 1);
            movementSubtask.RunTask(caller).OnTaskFinished.AddListener(AfterTask);
            _caller = caller;
            return this;
        }

        public void Update(CreatureController caller) 
        {
        
        }

        void AfterTask() 
        {
            if(movementSubtask != null)
                movementSubtask.EndTask(_caller);
            waitTask = new Wait(maxTime);
            waitTask.RunTask(_caller).OnTaskFinished.AddListener(Finished);
        }

        void Finished() 
        {
            if(waitTask != null)
                waitTask.EndTask(_caller);
            IsDone = true;
            OnTaskFinished.Invoke();
        }

        public Wander(float maxDistance, float maxTimeWait = 7, float minTimeWait = 0) 
        {
            distance = maxDistance;
            maxTime = new DataType.ValueRange(minTimeWait, maxTimeWait);
            IsDone = false;
            IsStarted = false;
            OnTaskFinished = new UnityEvent();
        }

        public Wander(float maxDistance, DataType.ValueRange range)
        {
            distance = maxDistance;
            maxTime = range;
            IsDone = false;
            IsStarted = false;
            OnTaskFinished = new UnityEvent();
        }
    }
}