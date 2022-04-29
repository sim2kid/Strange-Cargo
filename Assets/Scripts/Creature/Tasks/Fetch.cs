using Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Creature.Task
{
    public class Fetch : ITask
    {
        public bool IsDone { get; private set; }
        public bool IsStarted { get; private set; }
        public UnityEvent OnTaskFinished { get; private set; }
        System.Func<float> SatisResult;

        IHoldable ObjectToFetch;
        GameObject FetchObj => ((MonoBehaviour)ObjectToFetch).gameObject;
        GameObject Thrower;


        public float Satisfaction { get; private set; }
        public void SatisfactionHook(System.Func<float> func)
        {
            SatisResult = func;
        }

        public ITask RunTask(CreatureController caller)
        {
            IsDone = false;
            IsStarted = true;
            Satisfaction = 0;



            ITask gotoObject = new GoHere(FetchObj.transform, 1f);
            ITask pickUpObject = new Wait(1);
            ITask returnToThrower = new GoHere(Thrower.transform, 2f);

            // Must add in reverse order because it's a stack

            caller.AddSubTask(returnToThrower, (creatue, task) =>
            {
                Console.LogDebug($"Creature [{caller.Guid}]: Fetch - Dropping Object");
                caller.Mouth.LetGo();
                Satisfaction = 100;
                IsDone = true;
                OnTaskFinished.Invoke();
                return true;
            });

            caller.AddSubTask(pickUpObject, (creatue, task) => {
                Console.LogDebug($"Creature [{caller.Guid}]: Fetch - Returning to Thrower");
                caller.Mouth.PickUp(ObjectToFetch);
                Satisfaction = 50;
                return true;
            });

            caller.AddSubTask(gotoObject, (creatue, task) => {
                Console.LogDebug($"Creature [{caller.Guid}]: Fetch - Pickup Object!");
                caller.AnimationTrigger("Eat");
                return true;
            });

            return this;
        }

        public void EndTask(CreatureController caller)
        {
            caller.AnimationResetTrigger("Eat");
            if (caller.Mouth.HasObj) 
            {
                caller.Mouth.LetGo();
            }
            IsStarted = false;
            if (SatisResult != null)
                SatisResult.Invoke();
        }

        public void Update(CreatureController caller) { }

        public Fetch(IHoldable Fetchable, GameObject Origin)
        {
            if (!(Fetchable is MonoBehaviour)) 
            {
                Console.LogError("Fetch must be passed a Monobehavior for the holdable object.");
                return;
            }
            Thrower = Origin;
            IsStarted = false;
            ObjectToFetch = Fetchable;
            OnTaskFinished = new UnityEvent();
        }
    }

}