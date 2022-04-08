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
    public class SprayBottle : Pickupable, IUseable, INeedChange
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
        public UnityEvent onUseNotCreature;

        [SerializeField]
        private float useCooldown = 5f;
        float cooldown = 0;

        public void Use()
        {
            if (Creature != null && cooldown <= 0)
            {
                Creature.NegativeReinforcement();

                Creature.ProcessINeed(this);
                onUseCreature.Invoke();

                cooldown = useCooldown;
            }
            else 
            {
                onUseNotCreature.Invoke();
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
            if (cooldown > 0) 
            {
                cooldown -= Time.deltaTime;
            }

            IInteractable lastObj = Player.Interaction.Previous;
            if (lastObj != null)
            {
                Creature = lastObj.GameObject.GetComponent<CreatureController>();
            }
            else
            {
                Creature = null;
            }

            if (Creature != null && cooldown <= 0)
            {
                if (Creature.MemoryHasPreference)
                {
                    useString = $"{{use}} to Punish for {Creature.RecentMemory}";
                } 
                else 
                {
                    useString = $"{{use}} to Stop {Creature.RecentMemory}";
                }
            }
            else
            {
                useString = string.Empty;
            }
        }
    }
}