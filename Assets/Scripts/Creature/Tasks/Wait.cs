using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Creature.Task
{
    public class Wait : ITask
    {
        public bool IsDone { get; private set; }

        public bool IsStarted { get; private set; }

        public UnityEvent OnTaskFinished { get; private set; }

        private DataType.ValueRange waitInit;
        private float waitAmount;
        private float timer;

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
            waitInit = waitAmount;
            timer = 0;
            IsDone = false;
            IsStarted = true;
            caller.RequestMoreTaskTime(waitAmount);
            return this;
        }

        public void Update(CreatureController caller)
        {
            timer += Time.deltaTime;
            if (timer >= waitAmount)
            {
                IsDone = true;
                timer = -1;
                OnTaskFinished.Invoke();
            }
        }

        public Wait(float amount = 1f) 
        {
            IsDone = false;
            IsStarted = false;
            OnTaskFinished = new UnityEvent();
            waitAmount = amount;
            waitInit = amount;
        }

        public Wait(DataType.ValueRange amount)
        {
            IsDone = false;
            IsStarted = false;
            OnTaskFinished = new UnityEvent();
            waitAmount = (float)amount;
            waitInit = amount;
        }
    }
}