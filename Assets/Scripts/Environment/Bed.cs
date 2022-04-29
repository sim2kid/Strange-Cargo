using Creature.Brain;
using Creature.Stats;
using Creature.Task;
using DataType;
using PersistentData.Component;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    public class Bed : NeedSource, IObject
    {
        Sleep sleep;

        public override ITask RelatedTask => sleep;
        public override IObject RelatedObject => this;
        public string Name { get => gameObject.name; set { gameObject.name = value; } }
        public string Guid { get => this.GetComponent<PrefabSaveable>().Data.GUID; }

        void Awake()
        {
            sleep = new Sleep(this.transform, 3.5f);
        }

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
            return Guid == other.Guid;
        }
    }
}