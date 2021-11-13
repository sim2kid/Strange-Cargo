using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creature.Stats;
using Creature.Task;

namespace Creature.Brain
{
    [System.Serializable]
    public class BasicBrain
    {
        private CreatureController _controller;
        public BasicBrain(CreatureController controller) 
        {
            _controller = controller;
            coolDown = 0;
            Preferences = new List<float>[1];
            for(int i = 0; i< Preferences.Length; i++)
                Preferences[i] = new List<float>();
        }

        private float coolDown;

        [SerializeField]
        private List<float>[] Preferences;

        public void Think() 
        {
            if (coolDown > 0) 
            {
                coolDown -= Time.deltaTime;
            }
            if (_controller.TaskCount == 0)
            {
                float r = Random.Range(0f, 1f);
                if (r < 20f)
                {
                    _controller.AddTask(new Wander(5f));
                }
                else 
                {
                    _controller.AddTask(new Wait(Random.Range(7f,10f)));
                }
            }
            else if (_controller.TaskCount > 1 && _controller.TopTask.GetType() == typeof(Wander)) 
            {
                _controller.VoidTask();
            }

            if (coolDown > 0)
                return;

            if (_controller.needs.Appetite > 50) 
            {
                float wantFood = ((_controller.needs.Appetite / 200f) - (50/200))/10;
                float r = Random.Range(0f, 1f);
                if (r < wantFood) 
                {
                    coolDown += 50;
                    // HARDCODED FETCH OF FOODBOWLS
                    FoodBowl[] bowls = GameObject.FindObjectsOfType<FoodBowl>();
                    int toPick = pickFromPreferences(bowls.Length,(int)Biases.FoodBowl);
                    Eat eatTask = new Eat(bowls[toPick]);
                    _controller.AddTask(eatTask);
                }
            }
        }

        private int pickFromPreferences(int length, int bias) 
        {
            if (Preferences[bias].Count < length)
            {
                int diff = length - Preferences[bias].Count;
                for (int i = 0; i < diff; i++)
                    Preferences[bias].Add(1f);
            }
            else if (Preferences[bias].Count > length)
            {
                int diff = Preferences[bias].Count - 1;
                for (int i = diff; i >= length; i--)
                    Preferences[bias].RemoveAt(i);
            }

            List<int> list = generateRandomList(Preferences[bias]);

            int r = Random.Range(0, 100);
            int choice = list[r];

            Preferences[bias][choice] += 0.1f;

            return choice;
        }

        private List<int> generateRandomList(List<float> weights) 
        {
            List<int> list = new List<int>();
            float totalWeight = 0f;
            foreach (float w in weights)
                totalWeight += w;

            for (int w = 0; w < weights.Count; w++) 
            {
                int percent;

                if (totalWeight == 0)
                    percent = (1/weights.Count) * 100;
                else
                    percent = (int)Mathf.Round((weights[w] / totalWeight) * 100);

                for (int i = 0; i < percent; i++) 
                {
                    list.Add(w);
                }
            }
            return list;
        }
    }

    enum Biases 
    {
        FoodBowl = 0
    }
}