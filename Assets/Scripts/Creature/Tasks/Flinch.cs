using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Creature.Task
{
    public class Flinch : ITask
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

        public ITask RunTask(CreatureController caller)
        {
            IsDone = false;
            IsStarted = true;
            Satisfaction = 20;

            caller.AnimationTrigger("Flinch");
            caller.ProcessNeedChange(new Stats.Needs(0, 0, -30, 0, 0, 0));

            IsDone = true;

            return this;
        }

        public void EndTask(CreatureController caller)
        {
            IsStarted = false;
            if (SatisResult != null)
                SatisResult.Invoke();
            OnTaskFinished.Invoke();
        }

        public void Update(CreatureController caller) { }

        public Flinch()
        {
            IsStarted = false;
            OnTaskFinished = new UnityEvent();
        }
    }
}