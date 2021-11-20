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
        private bool calledFinished;

        CreatureController _caller;
        ITask come;
        ITask wait;
        UnityEvent _update;

        private FoodBowl _bowl;
        public ITask RunTask(CreatureController caller, UnityEvent update)
        {
            IsDone = false;
            IsStarted = true;
            _caller = caller;
            _update = update;

            come = new GoHere(_bowl.transform, 1f).RunTask(caller, update);
            come.OnTaskFinished.AddListener(EatTheBowl);

            calledFinished = false;
            update.AddListener(Update);
            return this;
        }

        private void EatTheBowl() 
        {
            Console.Log("At Bowl");
            come.EndTask(_update);

            wait = new Wait(5);
            wait.OnTaskFinished.AddListener(Finish);
            wait.RunTask(_caller, _update);
        }

        private void Finish() 
        {
            Console.Log("Eaten!");
            wait.EndTask(_update);

            _bowl.Eat(200 - _caller.needs.GetNeed(Need.Appetite));
            _caller.ProcessINeed(_bowl);

            IsDone = true;
        }

        public void EndTask(UnityEvent update)
        {
            update.RemoveListener(Update);
            wait.EndTask(update);
            come.EndTask(update);
            IsStarted = false;
        }

        private void Update()
        {
            if (IsDone && !calledFinished)
            {
                OnTaskFinished.Invoke();
                calledFinished = true;
            }
        }

        public Eat(FoodBowl bowl) 
        {
            IsStarted = false;
            _bowl = bowl;
            OnTaskFinished = new UnityEvent();
        }
    }
}