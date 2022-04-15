using Creature.Stats;
using Creature.Task;
using DataType;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
    [RequireComponent(typeof(AreaZone))]
    public class PoopZone : NeedSource, IObject
    {
        [SerializeField]
        public List<GameObject> PoopModels = new List<GameObject>();

        [SerializeField]
        public UnityEvent OnPoop;

        float multiplier = 0;
        public override Needs NeedChange => base.NeedChange * multiplier;
        public override Needs StatsEffect => base.NeedChange;
        public string Name { get => gameObject.name; set { gameObject.name = value; } }

        [SerializeField]
        private string _guid;
        public string Guid => _guid;

        private void OnEnable()
        {
            if (!Utility.Toolbox.Instance.AvalibleTasks.Contains(this))
                Utility.Toolbox.Instance.AvalibleTasks.Add(this);
        }

        private void OnDisable()
        {
            if (Utility.Toolbox.Instance.AvalibleTasks.Contains(this))
                Utility.Toolbox.Instance.AvalibleTasks.Remove(this);
        }

        public bool Equals(IObject other)
        {
            return (string.Equals(this.Name, other.Name) && string.Equals(this.Guid, other.Guid));
        }

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_guid)) 
            {
                _guid = System.Guid.NewGuid().ToString();
            }
        }

        public Vector3 GetPoint() 
        {
            return GetComponent<AreaZone>().GetRandomPoint();
        }

        public void Poop(Vector3 location, float amount) 
        {
            GameObject model = PoopModels[Random.Range(0, PoopModels.Count)];
            multiplier = amount;
            Instantiate(model, location + new Vector3(0,0.5f,0), Quaternion.identity);
        }

        public override ITask RelatedTask => new Poop(this);
        public override IObject RelatedObject => this;
    }
}