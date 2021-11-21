using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creature.Stats;
using Creature.Task;
using System.Linq;

namespace Creature.Brain
{
    [System.Serializable]
    public class BasicBrain
    {
        private CreatureController _controller;
        [SerializeField]
        private List<Preferred> Preferences;

        private Queue<System.Type> lastTasks;
        private int maxTrackSize;
        public BasicBrain(CreatureController controller) 
        {
            maxTrackSize = 50;
            _controller = controller;
            Preferences = new List<Preferred>();
            lastTasks = new Queue<System.Type>();
        }

        public void Think() 
        {
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
                if (task.RelatedObject != null)
                {
                    p = Preferences.Find(x => x.Obj == task.RelatedObject);
                    if (p != null)
                    {
                        preference = p.Preference;
                    }
                    else 
                    {
                        Preferences.Add(new Preferred(task.RelatedObject, 0f));
                    }
                }
                float CurrentUtility = Mathf.Clamp(preference, float.MinValue, 15f);

                // Add expected result
                if (task.StatsEffect != null)
                {
                    for (int i = 0; i < task.StatsEffect.Length; i++)
                    {
                        float expectedUtility = realitiveNeeds[i] * task.StatsEffect[i] * task.BaseUtility;
                        CurrentUtility += expectedUtility;
                    }
                }
                else 
                {
                    CurrentUtility += task.BaseUtility * 6;
                }

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
            // Track Task
            lastTasks.Enqueue(pickMe.Task.GetType());
        }

        private Needs getRealitiveNeeds() 
        {
            Needs f = _controller.needs;
            for (int i = 0; i < f.Count; i++) 
            {
                f[i] = (f[i] - f.Min) / (f.Max - f.Min);
                f[i] = 1 - f[i];
            }
            return f;
        }
    }
}