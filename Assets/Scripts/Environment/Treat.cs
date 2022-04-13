using Creature;
using Creature.Stats;
using Interaction;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
    public class Treat : Pickupable, IUseable, INeedChange
    {
        public UnityEvent OnUse;
        public string UseText => useString;

        public Needs NeedChange => needChange;

        public Needs needChange;

        private PlayerController Player;
        private Creature.CreatureController Creature;

        [SerializeField]
        private string useString;

        public UnityEvent onUseCreature;

        [SerializeField]
        private AudioClip eatSound;

        public void Use()
        {
            if (Creature != null)
            {
                Creature.PositiveReinforcement();

                Creature.ProcessINeed(this);
                onUseCreature.Invoke();

                Creature.AnimationTrigger("Eating");
            }
        }
        public void Mod1Use()
        {
            Use();
        }

        protected override void Start()
        {
            useString = string.Empty;
            Player = Utility.Toolbox.Instance.Player;
            base.Start();
        }

        public void HoldUpdate()
        {

            IInteractable lastObj = Player.Interaction.Previous;
            if (lastObj != null)
            {
                Creature = lastObj.GameObject.GetComponent<CreatureController>();
            }
            else
            {
                Creature = null;
            }

            if (Creature != null)
            {
                if (Creature.MemoryHasPreference)
                {
                    useString = $"{{use}} to Praise for {Creature.RecentMemory}";
                }
                else
                {
                    useString = $"{{use}} to Give Treat";
                }
            }
            else
            {
                useString = string.Empty;
            }
        }
    }
}