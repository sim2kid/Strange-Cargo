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
        [SerializeField]
        private string useOnCreature;

        public UnityEvent onEveryUse;
        public UnityEvent onUseCreature;
        public UnityEvent onUseNotCreature;

        public void Use()
        {
            onEveryUse.Invoke();
            if (Creature != null)
            {
                Creature.NegativeReinforcement();

                Creature.ProcessINeed(this);
                onUseCreature.Invoke();
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
            IInteractable lastObj = Player.Interaction.Previous;
            if (lastObj != null)
            {
                Creature = lastObj.GameObject.GetComponent<CreatureController>();
                if (Creature != null && Creature is CreatureController) 
                {
                    Creature = null;
                }
            }
            else
            {
                Creature = null;
            }



            if (Creature != null)
            {
                useString = $"{{use}} to Punish for {Creature.RecentMemory}";
            }
            else
            {
                useString = string.Empty;
            }
        }
    }
}