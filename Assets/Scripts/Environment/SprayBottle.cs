using Creature;
using Interaction;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
    public class SprayBottle : Pickupable, IUseable
    {
        public UnityEvent OnUse;
        public string UseText => useString;

        private PlayerController Player;
        private Creature.CreatureController Creature;

        [SerializeField]
        private string useString;
        [SerializeField]
        private string useOnCreature;

        public void Use()
        {
            //if (bowl != null)
            //{
            //    onuse.invoke();
            //    player.handcontroller.letgo();
            //    bowl.fill(200);
            //    destroy(this.gameobject);
            //}
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



            //if (Creature != null)
            //{
            //    useString = useOnBowl;// "{use} to Refill";
            //}
            //else
            //{
            //    useString = string.Empty;
            //}
        }
    }
}