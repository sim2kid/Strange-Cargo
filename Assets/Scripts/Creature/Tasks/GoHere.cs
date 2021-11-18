using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Creature.Task
{
    public class GoHere : ITask
    {
        public bool IsDone { get 
            {
                if (_caller == null)
                    return false;
                return Vector3.Distance(location, _caller.transform.position) < dis;
            } 
        }
        public UnityEvent OnTaskFinished { get; private set; }

        private bool calledFinished;
        private float dis;

        public bool IsStarted { get; private set; }

        public Vector3 location;
        CreatureController _caller; 
        public ITask RunTask(CreatureController caller, UnityEvent update)
        {
            _caller = caller;
            caller.Move.MoveTo(location);
            IsStarted = true;
            calledFinished = false;
            update.AddListener(Update);
            return this;
        }

        public void EndTask(UnityEvent update) 
        {
            update.RemoveListener(Update);
        }

        private void Update() 
        {
            if (IsDone && !calledFinished) 
            {
                OnTaskFinished.Invoke();
                calledFinished = true;
            }  
        }

        public GoHere(Transform destination, float minDistance = 1) : this(destination.position, minDistance) { }

        public GoHere(Vector3 destination, float minDistance = 1)
        {
            location = destination;
            IsStarted = false;
            dis = minDistance;
            OnTaskFinished = new UnityEvent();
        }
    }
}