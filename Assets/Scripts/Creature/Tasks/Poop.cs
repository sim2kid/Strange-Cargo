using Environment;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Creature.Task
{
    public class Poop : ITask
    {
        public bool IsDone { get; private set; }
        public bool IsStarted { get; private set; }
        public UnityEvent OnTaskFinished { get; private set; }
        PoopZone _poopZone;
        System.Func<float> SatisResult;
        public float Satisfaction { get; private set; }
        public void SatisfactionHook(System.Func<float> func)
        {
            SatisResult = func;
        }


        Vector3 poopPoint;
        CreatureController _caller;
        UnityEvent _update;

        ITask come, wait;

        public ITask RunTask(CreatureController caller, UnityEvent update)
        {
            IsDone = false;
            IsStarted = true;
            _caller = caller;
            _update = update;
            Satisfaction = 0;

            poopPoint = _poopZone.GetPoint();

            come = new GoHere(poopPoint, 1f).RunTask(caller, update);
            come.OnTaskFinished.AddListener(TakeAPoo);


            return this;
        }
        private void TakeAPoo() 
        {
            Console.LogDebug($"Creature [{_caller.Guid}]: Poop - In Poop Position");
            if (come != null)
                come.EndTask(_update);

            // Start poop animation

            wait = new Wait(3);
            wait.OnTaskFinished.AddListener(DonePooing);
            wait.RunTask(_caller, _update);

        }
        private void DonePooing() 
        {
            Console.LogDebug($"Creature [{_caller.Guid}]: Poop - Poop has been left!");
            if (wait != null)
                wait.EndTask(_update);

            _poopZone.Poop(_caller.transform.position, 200 - _caller.needs.Bladder);
            _caller.ProcessINeed(_poopZone);

            Satisfaction = 100;

            IsDone = true;
            OnTaskFinished.Invoke();
        }
        public void EndTask(UnityEvent update)
        {
            if (come != null)
                come.EndTask(update);
            if (wait != null)
                wait.EndTask(update);
            IsStarted = false;
            if (SatisResult != null)
                SatisResult.Invoke();
        }
        public Poop(PoopZone poopZone) 
        {
            IsStarted = false;
            _poopZone = poopZone;
            OnTaskFinished = new UnityEvent();
        }
    }
}