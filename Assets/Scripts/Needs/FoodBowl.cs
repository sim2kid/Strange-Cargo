using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataType;
using Environment;

namespace Creature.Stats
{
    [RequireComponent(typeof(Displacement))]
    public class FoodBowl : NeedSource, IValue
    {
        [Tooltip("Represents how full the bowl is in Appatite Points")]
        public float Fullness = 0;

        [Tooltip("Represents the max Appatite Points the bowl can hold")]
        public float MaxFullness = float.MaxValue;

        private float multiplier = 0;

        public float MaxValue => MaxFullness;
        public float MinValue => 0;

        private Displacement displacement;

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

        public float Read() 
        {
            return Fullness;
        }

        public void Eat(float amount) 
        {
            float temp = Mathf.Clamp(Fullness - amount, 0, float.MaxValue);
            multiplier = Fullness - temp;
            Fullness = Mathf.Clamp(Fullness - multiplier, 0 , float.MaxValue);
        }

        private void Start()
        {
            displacement = GetComponent<Displacement>();
            displacement.Value = this;
        }
    }
}