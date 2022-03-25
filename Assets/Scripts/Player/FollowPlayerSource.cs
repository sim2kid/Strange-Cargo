using Creature.Brain;
using Creature.Stats;
using Creature.Task;
using DataType;
using Environment;
using PersistentData.Component;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class FollowPlayerSource : MonoBehaviour, IUtility, IObject, INeedChange
    {
        public Needs NeedChange => StatsEffect * BaseUtility.Value;

        public ValueRange BaseUtility => 1;

        [SerializeField]
        private Needs _needs = new Needs(
            0,
            0,
            10,
            0,
            0,
            0);

        public Needs StatsEffect => _needs;

        public ITask RelatedTask => new Follow(this.transform, this);

        public IObject RelatedObject => this;

        public string Name { get => this.Name; set { } }

        public string Guid => GetComponentInParent<PersistentSaveable>().Data.GUID;

        public bool Equals(IObject other)
        {
            if (other == null || !(other is FollowPlayerSource) || other.Guid != this.Guid)
            {
                return false;
            }
            return true;
        }
    }
}