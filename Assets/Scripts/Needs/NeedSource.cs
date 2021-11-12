using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Stats
{
    public class NeedSource : MonoBehaviour, INeedChange
    {
        [SerializeField]
        protected float Appetite = 0;
        [SerializeField]
        protected float Bladder = 0;
        [SerializeField]
        protected float Social = 0;
        [SerializeField]
        protected float Energy = 0;
        [SerializeField]
        protected float Happiness = 0;
        [SerializeField]
        protected float Hygiene = 0;

        public virtual float[] NeedChange
        {
            get
            {
                return new float[]
                {
                    Appetite,
                    Bladder,
                    Social,
                    Energy,
                    Happiness,
                    Hygiene
                };
            }
        }
    }
}