using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Creature.Stats
{
    [Serializable]
    public class Needs
    {
        private float min = 0;
        private float max = 200;
        [SerializeField]
        float[] _myNeeds = new float[6];

        public float Appetite => _myNeeds[0];
        public float Bladder => _myNeeds[1];
        public float Social => _myNeeds[2];
        public float Energy => _myNeeds[3];
        public float Happiness => _myNeeds[4];
        public float Hygiene => _myNeeds[5];

        public float[] RawNeeds => GetNeeds();

        public Needs() 
        {
            SetNeeds(((max-min)/2)+min);
        }

        public float GetNeed(Need need)
        {
            return _myNeeds[(int)need];
        }
        public float[] GetNeeds()
        {
            return (float[])_myNeeds.Clone();
        }

        public float AddNeed(Need need, float amount) 
        {
            return _myNeeds[(int)need] = Mathf.Clamp(_myNeeds[(int)need] + amount, min, max);
        }
        public float[] AddNeeds(float[] amounts)
        {
            for (int i = 0; i < _myNeeds.Length; i++)
               _myNeeds[i] = Mathf.Clamp(_myNeeds[i] + amounts[i], min, max);
            return (float[])_myNeeds.Clone();
        }
        public float[] AddNeeds(float amount)
        {
            for (int i = 0; i < _myNeeds.Length; i++)
                _myNeeds[i] = Mathf.Clamp(_myNeeds[i] + amount, min, max);
            return (float[])_myNeeds.Clone();
        }

        public float SetNeed(Need need, float amount)
        {
            return _myNeeds[(int)need] = Mathf.Clamp(amount, min, max);
        }

        public float[] SetNeeds(float[] amounts)
        {
            for (int i = 0; i < _myNeeds.Length; i++)
                _myNeeds[i] = Mathf.Clamp(amounts[i], min, max);
            return (float[])_myNeeds.Clone();
        }

        public float[] SetNeeds(float amount)
        {
            for (int i = 0; i < _myNeeds.Length; i++)
                _myNeeds[i] = Mathf.Clamp(amount, min, max);
            return (float[])_myNeeds.Clone();
        }
    }
}