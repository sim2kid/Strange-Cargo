using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creature.Brain;

namespace Creature.Stats
{
    public class NeedSource : MonoBehaviour, INeedChange, IUtility
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
        protected float Hygiene = 0;
        [SerializeField]
        protected float Happiness = 0;

        [SerializeField]
        protected float UtilityRate = 0;

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
                    Hygiene,
                    Happiness
                };
            }
        }

        public virtual float[] StatsEffect => NeedChange;
        public virtual Environment.IObject RelatedObject => null;

        private void Start()
        {
            if(this.GetType() == typeof(NeedSource)) 
            {
                Debug.LogWarning($"{this.GetType()} should not be used directly in a scene. Please extend the class!");
            }
        }

        public virtual float BaseUtility => UtilityRate;
        public virtual Creature.Task.ITask RelatedTask => null;
    }
}