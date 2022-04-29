using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Creature.Task
{
    public class Sleep : ITask
    {
        public bool IsDone { get; private set; }
        public bool IsStarted { get; private set; }
        public UnityEvent OnTaskFinished { get; private set; }
        System.Func<float> SatisResult;

        float sleepToGo;
        float sleepQuality = 1f;

        Transform bed;
        bool inBed;

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
            totalSleep = 0;
            inBed = false;

            ITask gotoBed = new GoHere(bed, 0.5f);

            caller.AddSubTask(gotoBed, (creature, task) => {
            Console.LogDebug($"Creature [{caller.Guid}]: Sleep - Creature in bed.");

            sleepToGo = caller.needs.Max - caller.needs.Energy;
            caller.AnimationBool("IsSleeping", true);

            caller.RequestMoreTaskTime(10 + (sleepToGo / sleepQuality));

                // Set face to sleep too

                inBed = true;

                return true;
            });
            return this;
        }

        public void EndTask(CreatureController caller)
        {
            caller.AnimationBool("IsSleeping", false);
            IsStarted = false;
            if (SatisResult != null)
                SatisResult.Invoke();
            OnTaskFinished.Invoke();
        }

        float totalSleep = 0;

        public void Update(CreatureController caller) 
        {
            if (!inBed) return;
            float sleepForFrame = Time.deltaTime * sleepQuality;
            totalSleep += sleepForFrame;
            caller.ProcessNeedChange(new Stats.Needs(0, 0, 0, sleepForFrame, 0, 0));
            Satisfaction = (totalSleep / sleepToGo) * 100;
            if (caller.needs.Energy > 200f) // Stop when well rested
            {
                IsDone = true;
            } 
            else if (caller.needs.Energy > 160f && Random.Range(0,1f) < 0.01f) // 1% chance per frame to stop sleeping
            {
                Satisfaction -= 10f;
                IsDone = true;
            }
        }

        public Sleep(Transform sleepLocation, float sleepQuality = 1f)
        {
            bed = sleepLocation;
            this.sleepQuality = sleepQuality;
            IsStarted = false;
            OnTaskFinished = new UnityEvent();
        }
    }
}