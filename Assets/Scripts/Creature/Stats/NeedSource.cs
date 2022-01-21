using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creature.Brain;
using DataType;

namespace Creature.Stats
{
    public class NeedSource : MonoBehaviour, INeedChange, IUtility
    {
        [Header("Needs")]
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

        [Header("Utility")]
        [SerializeField]
        protected float UtilityRate = 0;

        public virtual Needs NeedChange { get => new Needs(Appetite, Bladder, Social, Energy, Hygiene, Happiness);
            set 
            {
                Appetite = value.Appetite;
                Bladder = value.Bladder;
                Social = value.Social;
                Energy = value.Energy;
                Hygiene = value.Hygiene;
                Happiness = value.Happiness;
            }
        }
        public virtual Needs StatsEffect => NeedChange;
        public virtual Environment.IObject RelatedObject => null;

        private void Start()
        {
            if(this.GetType() == typeof(NeedSource)) 
            {
                Debug.LogWarning($"{this.GetType()} should not be used directly in a scene. Please extend the class!");
            }
        }

        public virtual ValueRange BaseUtility => UtilityRate;
        public virtual Creature.Task.ITask RelatedTask => null;
    }
}