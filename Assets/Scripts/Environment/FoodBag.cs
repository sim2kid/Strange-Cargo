using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interaction;
using Player;
using Creature.Stats;

namespace Environment {
    public class FoodBag : Pickupable, IUseable
    {
        
        public string UseText => useString;

        private PlayerController Player;
        private FoodBowl Bowl;

        private string useString;

        public void Use()
        {
            if (Bowl != null)
            {
                Player.HandController.LetGo();
                Bowl.Fill(200);
                Destroy(this);
            }
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
            Bowl = lastObj.GameObject.GetComponent<FoodBowl>();
            if (Bowl != null)
            {
                useString = "{use} to Refill";
            }
            else 
            {
                useString = string.Empty;
            }
        }
    }
}