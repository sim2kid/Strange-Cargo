using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Creature.Stats
{
    [Serializable]
    public class Needs
    {
        public float Min { get; private set; }
        public float Max { get; private set; }
        [SerializeField]
        float[] _myNeeds = new float[6];

        public float Appetite => _myNeeds[0];
        public float Bladder => _myNeeds[1];
        public float Social => _myNeeds[2];
        public float Energy => _myNeeds[3];
        public float Hygiene => _myNeeds[4];
        public float Happiness => _myNeeds[5];

        public float[] RawNeeds => GetNeeds();

        public Needs() 
        {
            Min = 0;
            Max = 200;
            SetNeeds(Max);
    }

        public float GetNeed(Need need)
        {
            return GetNeed((int)need);
        }
        public float GetNeed(int need)
        {
            return _myNeeds[need];
        }
        public float[] GetNeeds()
        {
            return (float[])_myNeeds.Clone();
        }



        public float AddNeed(Need need, float amount) 
        {
            return _myNeeds[(int)need] = Mathf.Clamp(_myNeeds[(int)need] + amount, Min, Max);
        }
        public float[] AddNeeds(float[] amounts)
        {
            for (int i = 0; i < _myNeeds.Length; i++)
               _myNeeds[i] = Mathf.Clamp(_myNeeds[i] + amounts[i], Min, Max);
            return GetNeeds();
        }
        public float[] AddNeeds(float appetite, float bladder, float social, float energy, float hygiene, float happiness)
        {
            return AddNeeds(new float[] { appetite, bladder, social, energy, hygiene, happiness });
        }
        public float[] AddNeeds(float amount)
        {
            return AddNeeds(amount, amount, amount, amount, amount, amount);
        }



        public float SetNeed(Need need, float amount)
        {
            return _myNeeds[(int)need] = Mathf.Clamp(amount, Min, Max);
        }

        public float[] SetNeeds(float[] amounts)
        {
            for (int i = 0; i < _myNeeds.Length; i++)
                _myNeeds[i] = Mathf.Clamp(amounts[i], Min, Max);
            return GetNeeds();
        }
        public float[] SetNeeds(float appetite, float bladder, float social, float energy, float hygiene, float happiness)
        {
            return SetNeeds(new float[] { appetite, bladder, social, energy, hygiene, happiness });
        }
        public float[] SetNeeds(float amount)
        {
            return SetNeeds(amount, amount, amount, amount, amount, amount);
        }
    }
}