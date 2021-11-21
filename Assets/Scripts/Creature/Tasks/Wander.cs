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

        public void EndTask(UnityEvent update)
        {
            IsStarted = false;
        }

        public ITask RunTask(CreatureController caller, UnityEvent update)
        {
            IsStarted = true;
            IsDone = false;
            movementSubtask = new GoHere(caller.transform.position + (Random.insideUnitSphere * distance), 1);
            movementSubtask.RunTask(caller, update).OnTaskFinished.AddListener(AfterTask);
            _caller = caller;
            _update = update;
            return this;
        }

        void AfterTask() 
        {
            if(movementSubtask != null)
                movementSubtask.EndTask(_update);
            waitTask = new Wait(maxTime);
            waitTask.RunTask(_caller, _update).OnTaskFinished.AddListener(Finished);
        }

        void Finished() 
        {
            if(waitTask != null)
                waitTask.EndTask(_update);
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