using Creature.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Creature.Task
{
    public class Eat : ITask
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

        private bool calledFinished;

        CreatureController _caller;
        ITask come;
        ITask wait;

        private FoodBowl _bowl;
        public ITask RunTask(CreatureController caller)
        {
            IsDone = false;
            IsStarted = true;
            _caller = caller;
            Satisfaction = 0;

            come = new GoHere(_bowl.transform, 1f).RunTask(caller);
            come.OnTaskFinished.AddListener(EatTheBowl);

            calledFinished = false;
            return this;
        }

        private void EatTheBowl() 
        {
            Console.LogDebug($"Creature [{_caller.Guid}]: Eating - At Bowl");
            come.EndTask(_caller);

            _caller.AnimationBool("Eat", true);
            _bowl.BeforeEat();

            wait = new Wait(5);
            wait.OnTaskFinished.AddListener(Finish);
            wait.RunTask(_caller);
        }

        private void Finish() 
        {
            wait.EndTask(_caller);

            _caller.AnimationBool("Eat", false);
            _bowl.Eat(200 - _caller.needs.Appetite);
            _caller.ProcessINeed(_bowl);
            Satisfaction = 100;

            Console.LogDebug($"Creature [{_caller.Guid}]: Eating - Finished eating! New Appetite: {_caller.needs.Appetite}");

            IsDone = true;
        }

        public void EndTask(CreatureController caller)
        {
            _caller.AnimationBool("Eat", false);
            if (wait != null)
                wait.EndTask(caller);
            if(come != null)
                come.EndTask(caller);
            IsStarted = false;
            if (SatisResult != null)
                SatisResult.Invoke();
        }

        public void Update(CreatureController caller)
        {
            if (wait != null)
                wait.Update(caller);
            if (come != null)
                come.Update(caller);
            if (IsDone && !calledFinished)
            {
                OnTaskFinished.Invoke();
                calledFinished = true;
            }
        }

        public Eat(FoodBowl bowl) 
        {
            Console.HideInDebugConsole();
            IsStarted = false;
            _bowl = bowl;
            OnTaskFinished = new UnityEvent();
        }
    }
}