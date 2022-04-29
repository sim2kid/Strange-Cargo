using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creature.Stats;
using Creature.Task;
using System.Linq;
using Newtonsoft.Json;

namespace Creature.Brain
{
    [System.Serializable]
    public class BasicBrain
    {
        private CreatureController _controller;
        [SerializeField]
        [JsonProperty]
        private List<Preferred> Preferences;
        [SerializeField]
        [JsonProperty]
        private Queue<System.Type> lastTasks;
        private int maxTrackSize;

        private Memory lastMemory;
        private Memory currentMemory;
        [JsonIgnore]
        public Option RecentMemory => defineRecentMemory();

        private Option defineRecentMemory() 
        {
            if (lastMemory == null) 
            {
                if (currentMemory != null) 
                {
                    return currentMemory.option;
                }
                return null;
            }
            if (lastMemory.option.Preferrence != null && lastMemory.timeEnded + 5 > Time.time)
            {
                return lastMemory.option;
            }
            return currentMemory.option;
        }

        public BasicBrain(CreatureController controller) 
        {
            maxTrackSize = 50;
            _controller = controller;
            Preferences = new List<Preferred>();
            lastTasks = new Queue<System.Type>();
        }

        public void PositiveReinforcement(Option option) 
        {
            if (option.Preferrence != null)
                option.Preferrence.Preference += 0.02f;
        }

        public void NegativeReinforcement(Option option)
        {
            if (currentMemory != null && option.Preferrence != null 
                && currentMemory.option.Preferrence != null && option.Preferrence.Guid.Equals(currentMemory.option.Preferrence.Guid))
            {
                _controller.VoidTask();
            }

            if (option.Preferrence != null)
                option.Preferrence.Preference -= 0.05f;
        }

        public void Think() 
        {
            //var watch = new System.Diagnostics.Stopwatch();
            //watch.Start();

            if (lastTasks.Count >= maxTrackSize) 
            {
                lastTasks.Dequeue();
            }

            List<IUtility> potentialTasks = Utility.Toolbox.Instance.AvalibleTasks;
            List<Option> options = new List<Option>();

            Needs realitiveNeeds = getRealitiveNeeds();

            // Figure out Utility Per Task
            foreach (IUtility task in potentialTasks) 
            {
                // Add preference
                float preference = 0;
                Preferred p = null;
                if (task == null) { continue; }
                if (task.RelatedObject != null)
                {
                    p = Preferences.Find(x => x.Guid.Equals(task.RelatedObject.Guid));
                    if (p != null)
                    {
                        preference = p.Preference;
                    }
                    else 
                    {
                        Preferences.Add(new Preferred(task.RelatedObject, task.GetType().ToString(), 0f));
                    }
                }
                float CurrentUtility = Mathf.Clamp(preference, float.MinValue, 15f);

                // Add expected result
                if (task.StatsEffect != Needs.Zero)
                {
                    for (int i = 0; i < task.StatsEffect.Count; i++)
                    {
                        float expectedUtility = realitiveNeeds[i] * task.StatsEffect[i] * (float)task.BaseUtility;
                        CurrentUtility += expectedUtility;
                    }
                }
                else 
                {
                    CurrentUtility += (float)task.BaseUtility * 6;
                }
                // Add some noise
                CurrentUtility += Random.value * 1f;


                // Add Diminishing Returns
                System.Type taskType = task.RelatedTask.GetType();
                int count = 0;
                foreach (System.Type type in lastTasks) 
                {
                    if (type == taskType)
                        count++;
                }
                CurrentUtility /= (count * 0.3f * count) + 1;


                // Add new option to list for sorting later
                options.Add(new Option(CurrentUtility, task.RelatedTask, p));
            }
            

            // Look at task with most utility
            Option pickMe = options.Max();
            // Run that task
            _controller.AddTask(pickMe.Task);
            // Update preferrence
            if (pickMe.Preferrence != null)
                pickMe.Preferrence.Preference += 0.01f;
            // Label last option
            lastMemory = currentMemory;
            if (lastMemory != null)
            {
                lastMemory.timeEnded = Time.time;
            }
            currentMemory = new Memory(pickMe);

            // Track Task
            lastTasks.Enqueue(pickMe.Task.GetType());

            //watch.Stop();
            //Console.LogDebug($"Think time: {watch.ElapsedMilliseconds}ms");
        }

        /// <summary>
        /// Returns the needs from a range of [0-1] and inverted (Unit Needs)
        /// </summary>
        /// <returns></returns>
        private Needs getRealitiveNeeds() 
        {
            Needs f = _controller.needs.Clone();
            for (int i = 0; i < f.Count; i++) 
            {
                f[i] = (f[i] - f.Min) / (f.Max - f.Min);
                f[i] = 1 - f[i];
            }
            return f;
        }

        public void PreSerialization()
        {
            return;
        }

        public void PreDeserialization()
        {
            return;
        }

        public void PostDeserialization(CreatureController controller)
        {
            _controller = controller;
            foreach (var p in Preferences) 
            {
                p.Update();
            }
            return;
        }
    }
}