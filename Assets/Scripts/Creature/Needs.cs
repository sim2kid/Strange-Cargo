using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature
{
    public class Needs
    {
        float[] _myNeeds = new float[6];

        public float GetNeed(Need need)
        {
            return _myNeeds[(int)need];
        }

        public float AddNeed(Need need, float amount) 
        {
            return _myNeeds[(int)need] += amount;
        }

        public float SetNeed(Need need, float amount)
        {
            return _myNeeds[(int)need] = amount;
        }
    }
}