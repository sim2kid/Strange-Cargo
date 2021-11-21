using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataType;
using Environment;
using Creature.Brain;
using System;
using UnityEngine.Events;

namespace Creature.Stats
{
    [Serializable]
    [RequireComponent(typeof(Displacement))]
    public class FoodBowl : NeedSource, IValue, IObject
    {
        [SerializeField]
        [Tooltip("Represents how full the bowl is in Appatite Points")]
        public float Fullness = 0;

        [SerializeField]
        [Tooltip("Represents the max Appatite Points the bowl can hold")]
        public float MaxFullness = float.MaxValue;

        [SerializeField]
        public UnityEvent OnFill;

        [SerializeField]
        public UnityEvent OnEat;

        [SerializeField][HideInInspector]
        private Guid _guid;

        private float multiplier = 0;

        public float MaxValue => MaxFullness;
        public float MinValue => 0;

        private Displacement displacement;

        public override Needs NeedChange => base.NeedChange * multiplier;

        public override float BaseUtility => (base.BaseUtility * 0.5f) + ((Fullness / MaxValue) * 0.5f);
        public override Creature.Task.ITask RelatedTask => new Creature.Task.Eat(this);
        public override Needs StatsEffect => base.NeedChange;
        public override IObject RelatedObject => this;

        

        public string Name { get => gameObject.name; set { gameObject.name = value; } }
        public string Guid => _guid.ToString();


        public float Read() 
        {
            return Fullness;
        }

        public void Eat(float amount) 
        {
            float temp = Mathf.Clamp(Fullness - amount, 0, float.MaxValue);
            multiplier = Fullness - temp;
            Fullness = Mathf.Clamp(Fullness - multiplier, 0 , float.MaxValue);
            OnEat.Invoke();
        }

        public void Fill(float amount) 
        {
            Fullness = Mathf.Clamp(amount, MinValue, MaxValue);
            OnFill.Invoke();
        }

        private void OnEnable()
        {
            _guid = new Guid();
        }

        private void Start()
        {
            displacement = GetComponent<Displacement>();
            displacement.Value = this;
            Utility.Toolbox.Instance.AvalibleTasks.Add(this);
        }

        public bool Equals(IObject other)
        {
            return (string.Equals(this.Name, other.Name) && string.Equals(this.Guid, other.Guid));
        }
    }
}