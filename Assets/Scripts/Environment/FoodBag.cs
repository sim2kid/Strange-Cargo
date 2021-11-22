using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interaction;
using Player;
using Creature.Stats;
using UnityEngine.Events;

namespace Environment {

    [RequireComponent(typeof(Sound.AudioPlayer))]
    public class FoodBag : Pickupable, IUseable
    {
        public UnityEvent OnUse;
        public string UseText => useString;

        private PlayerController Player;
        private FoodBowl Bowl;

        private string useString;

        public void Use()
        {
            if (Bowl != null)
            {
                OnUse.Invoke();
                Player.HandController.LetGo();
                Bowl.Fill(200);
                Destroy(this.gameObject);
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
            if (lastObj != null)
                Bowl = lastObj.GameObject.GetComponent<FoodBowl>();
            else 
                Bowl = null;

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