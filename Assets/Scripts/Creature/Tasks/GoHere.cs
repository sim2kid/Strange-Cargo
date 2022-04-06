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

        bool checkedPath = false;

        System.Func<float> SatisResult;
        public float Satisfaction { get; private set; }
        public void SatisfactionHook(System.Func<float> func)
        {
            SatisResult = func;
        }
        public ITask RunTask(CreatureController caller)
        {
            _caller = caller;
            caller.Move.MoveTo(location);
            IsStarted = true;
            calledFinished = false;
            checkedPath = false;

            return this;
        }

        public void EndTask(CreatureController caller) 
        {
            caller.Move.ClearDestination();
            IsStarted = false;
            if(SatisResult != null)
                SatisResult.Invoke();
        }

        public void Update(CreatureController caller) 
        {
            if (IsDone && !calledFinished) 
            {
                caller.Move.ClearDestination();
                OnTaskFinished.Invoke();
                calledFinished = true;
                Satisfaction = 100;
            }
            if (!checkedPath && !_caller.Move.pathPending) 
            {
                checkedPath = true;
                Console.LogDebug($"Creature [{caller.Guid}]: GoHere - New Location [{location}]. Expected Walk Distance [{caller.Move.Distance.ToString("0.00")}]");

                if(caller.Move.Distance < Mathf.Infinity)
                    caller.RequestMoreTaskTime(caller.Move.Distance * 1.5f);
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