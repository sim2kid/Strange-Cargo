using Creature.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Creature.Task
{
    public class Follow : ITask
    {
        public bool IsDone { get; private set; }

        public bool IsStarted { get; private set; }

        public UnityEvent OnTaskFinished { get; private set; }

        System.Func<float> SatisResult;
        public float Satisfaction { get; private set; }
        public void SatisfactionHook(System.Func<float> func)
        {
            SatisResult = func;
        }

        CreatureController _caller;
        INeedChange _util;
        private bool calledFinished;

        private Transform _followMe;
        public ITask RunTask(CreatureController caller, UnityEvent update)
        {
            IsDone = false;
            IsStarted = true;
            Satisfaction = 100;
            _caller = caller;

            update.AddListener(Update);

            return this;
        }

        public void EndTask(UnityEvent update)
        {
            update.RemoveListener(Update);

            if (!_caller.Move.CanReachDestination)
            {
                Satisfaction = 30;
            }
            else 
            {
                _caller.ProcessINeed(_util);
            }

            IsStarted = false;
            if (SatisResult != null)
                SatisResult.Invoke();
        }

        private void Update()
        {
            _caller.Move.MoveTo(_followMe);
        }

        public Follow(Transform whoToFollow, INeedChange util)
        {
            _util = util;
            Console.HideInDebugConsole();
            IsStarted = false;
            _followMe = whoToFollow;
            OnTaskFinished = new UnityEvent();
        }
    }
}