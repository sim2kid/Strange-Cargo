using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Stats
{
    public class FoodBowl : NeedSource
    {
        [Tooltip("Represents how full the bowl is in Appatite Points")]
        public float Fullness = 0;

        private float multiplier = 0;

        public override float[] NeedChange
        {
            get
            {
                return new float[]
                {
                    Appetite * multiplier,
                    Bladder * multiplier,
                    Social * multiplier,
                    Energy * multiplier,
                    Happiness * multiplier,
                    Hygiene * multiplier
                };
            }
        }

        public void Eat(float amount) 
        {
            float temp = Mathf.Clamp(Fullness - amount, 0, float.MaxValue);
            multiplier = Fullness - temp;
            Fullness = Mathf.Clamp(Fullness - multiplier, 0 , float.MaxValue);
        }
    }
}