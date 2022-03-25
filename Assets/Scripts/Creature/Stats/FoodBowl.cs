using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataType;
using Environment;
using Creature.Brain;
using System;
using UnityEngine.Events;
using PersistentData.Saving;
using PersistentData.Component;

namespace Creature.Stats
{
    [Serializable]
    [RequireComponent(typeof(Displacement))]
    public class FoodBowl : NeedSource, IValue, IObject, ISaveable
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

        [SerializeField]
        public UnityEvent OnBeforeEat;

        [SerializeField]
        public UnityEvent OnEmpty;

        private float multiplier = 0;

        public float MaxValue => MaxFullness;
        public float MinValue => 0;

        private Displacement displacement;

        public override Needs NeedChange => base.NeedChange * multiplier;

        public override ValueRange BaseUtility => (base.BaseUtility * 0.5f) + ((Fullness / MaxValue) * 0.5f);
        public override Creature.Task.ITask RelatedTask => new Creature.Task.Eat(this);
        public override Needs StatsEffect => base.NeedChange;
        public override IObject RelatedObject => this;



        public string Name { get => gameObject.name; set { gameObject.name = value; } }
        public string Guid { get => GetComponentInParent<PrefabSaveable>().Data.GUID;  }
        public ISaveData saveData { get => data; set => data = (NeedSourceData)value; }

        [SerializeField]
        NeedSourceData data;

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(data._guid))
            {
                data._guid = System.Guid.NewGuid().ToString();
            }
        }

        public float Read() 
        {
            return Fullness;
        }

        public void BeforeEat() 
        {
            OnBeforeEat.Invoke();
        }

        public void Eat(float amount) 
        {
            float temp = Mathf.Clamp(Fullness - amount, 0, float.MaxValue);
            multiplier = Fullness - temp;
            Fullness = Mathf.Clamp(Fullness - multiplier, 0 , float.MaxValue);
            OnEat.Invoke();
            Console.LogDebug($"Bowl {name} with the guid of [{Guid}] has been eaten out of for {amount} and now only has {Fullness}/{MaxValue} left.");
            if (Fullness <= 0)
                Empty();
        }

        private void Empty() 
        {
            Console.LogDebug($"Bowl {name} with the guid of [{Guid}] is now Empty.");
            if (Utility.Toolbox.Instance.AvalibleTasks.Contains(this))
                Utility.Toolbox.Instance.AvalibleTasks.Remove(this);
            OnEmpty.Invoke();
        }

        public void Fill(float amount) 
        {
            Fullness = Mathf.Clamp(amount, MinValue, MaxValue);
            if (Fullness > 0 && !Utility.Toolbox.Instance.AvalibleTasks.Contains(this))
                Utility.Toolbox.Instance.AvalibleTasks.Add(this);
            Console.LogDebug($"Bowl {name} with the guid of [{Guid}] has been filled up by {amount} and now only has {Fullness}/{MaxValue} food.");
            OnFill.Invoke();
        }

        private void OnEnable()
        {
            if(!Utility.Toolbox.Instance.AvalibleTasks.Contains(this))
                Utility.Toolbox.Instance.AvalibleTasks.Add(this);
        }

        private void OnDisable()
        {
            if(Utility.Toolbox.Instance.AvalibleTasks.Contains(this))
                Utility.Toolbox.Instance.AvalibleTasks.Remove(this);
        }

        private void Awake()
        {
            Console.HideInDebugConsole();
        }

        private void Start()
        {
            displacement = GetComponent<Displacement>();
            displacement.Value = this;
        }

        public bool Equals(IObject other)
        {
            return (string.Equals(this.Name, other.Name) && string.Equals(this.Guid, other.Guid));
        }

        public void PreSerialization()
        {
            data.BaseNeeds = base.NeedChange;
            data.Fullness = this.Fullness;
        }

        public void PreDeserialization()
        {
            return;
        }

        public void PostDeserialization()
        {
            base.NeedChange = data.BaseNeeds;
            Fullness = data.Fullness;
        }
    }
}