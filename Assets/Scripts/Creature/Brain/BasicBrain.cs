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
        }

        public void Think() 
        {
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
        }
    }
}