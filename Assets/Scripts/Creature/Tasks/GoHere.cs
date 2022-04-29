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

        private Transform transLoc;
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
            if (transLoc != null)
            {
                location = transLoc.position;
            }
            _caller = caller;
            caller.Move.MoveTo(location);
            startLoc = location;
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

        private Vector3 startLoc;
        private float LastDistance = 0;

        public void Update(CreatureController caller) 
        {
            if (transLoc != null) 
            {
                location = transLoc.position;
                if (Vector3.Distance(startLoc, location) > 2f) 
                {
                    LastDistance = caller.Move.Distance;
                    startLoc = location;
                    caller.Move.MoveTo(location);
                    checkedPath = false;
                }
            }
            if (IsDone && !calledFinished) 
            {
                caller.Move.ClearDestination();
                OnTaskFinished.Invoke();
                calledFinished = true;
                Satisfaction = 100;
            }
            if (!checkedPath &&
                !_caller.Move.pathPending && 
                _caller.Move.pathStatus != UnityEngine.AI.NavMeshPathStatus.PathInvalid) 
            {
                checkedPath = true;
                Console.LogDebug($"Creature [{caller.Guid}]: GoHere - New Location [{location}]. Expected Walk Distance [{caller.Move.Distance.ToString("0.00")}]");

                

                if (caller.Move.Distance < Mathf.Infinity)
                    caller.RequestMoreTaskTime((caller.Move.Distance - LastDistance) * 1.5f);
            }
        }

        public GoHere(Transform destination, float minDistance = 1) 
        {
            transLoc = destination;
            location = destination.position;
            IsStarted = false;
            dis = minDistance;
            OnTaskFinished = new UnityEvent();
        }

        public GoHere(Vector3 destination, float minDistance = 1)
        {
            transLoc = null;
            location = destination;
            IsStarted = false;
            dis = minDistance;
            OnTaskFinished = new UnityEvent();
        }
    }
}