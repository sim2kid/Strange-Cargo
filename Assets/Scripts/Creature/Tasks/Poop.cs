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

        public ITask RunTask(CreatureController caller)
        {
            IsDone = false;
            IsStarted = true;
            Satisfaction = 0;

            poopPoint = _poopZone.GetPoint();

            ITask come = new GoHere(poopPoint, 1f);
            ITask wait = new Wait(3);

            // Must add in reverse order because it's a stack

            caller.AddSubTask(wait, (creatue, task) =>
            {
                DonePooing(creatue);
                return true;
            });

            caller.AddSubTask(come, (creatue, task) => {
                TakeAPoo(creatue);
                return true;
            });
            
            return this;
        }
        private void TakeAPoo(CreatureController caller) 
        {
            Console.LogDebug($"Creature [{caller.Guid}]: Poop - In Poop Position");
            caller.AnimationBool("Wiggle", true);
        }
        private void DonePooing(CreatureController caller) 
        {
            Console.LogDebug($"Creature [{caller.Guid}]: Poop - Poop has been left!");

            _poopZone.Poop(caller.transform.position, 200 - caller.needs.Bladder);
            caller.ProcessINeed(_poopZone);
            caller.AnimationBool("Wiggle", false);

            Satisfaction = 100;

            IsDone = true;
            OnTaskFinished.Invoke();
        }
        public void EndTask(CreatureController caller)
        {
            caller.AnimationBool("Wiggle", false);
            IsStarted = false;
            if (SatisResult != null)
                SatisResult.Invoke();
        }

        public void Update(CreatureController caller) { }

        public Poop(PoopZone poopZone) 
        {
            IsStarted = false;
            _poopZone = poopZone;
            OnTaskFinished = new UnityEvent();
        }
    }
}