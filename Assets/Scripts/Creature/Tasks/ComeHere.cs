using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Task
{
    public class ComeHere : ITask
    {
        public bool IsDone { get 
            {
                if (_caller == null)
                    return false;
                return Vector3.Distance(location, _caller.transform.position) < 1 || _caller.Move.Speed == 0f;
            } 
        }

        public bool IsStarted { get; private set; }

        public Vector3 location;
        CreatureController _caller; 
        public void RunTask(CreatureController caller)
        {
            _caller = caller;
            caller.Move.MoveTo(location);
            IsStarted = true;
        }

        public ComeHere(Transform destination) : this(destination.position) { }

        public ComeHere(Vector3 destination)
        {
            location = destination;
            IsStarted = false;
        }
    }
}