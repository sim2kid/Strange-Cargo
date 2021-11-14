using Creature.Task;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Brain
{
    public class Option : IComparable<Option>
    {
        public float Utility;
        public ITask Task;
        public Preferred Preferrence;
        public Option(float utility, ITask task, Preferred preferred) 
        {
            Utility = utility;
            Task = task;
            Preferrence = preferred;
        }

        public int CompareTo(Option other)
        {
            return Utility.CompareTo(other.Utility);
        }
    }
}