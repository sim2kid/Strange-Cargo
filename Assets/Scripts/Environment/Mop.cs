using Interaction;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
    public class Mop : Pickupable, IUseable
    {
        public UnityEvent OnUse;
        public string UseText => useString;

        private PlayerController Player;
        private Pee Pee;

        private string useString;
        [SerializeField]
        private string useOnPee;

        public void Use()
        {
            if (Pee != null)
            {
                OnUse.Invoke();
                Pee.Clean();
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
                Pee = lastObj.GameObject.GetComponent<Pee>();
                if (Pee != null)
                    if (Pee.GetType() != typeof(Pee))
                        Pee = null;
            }
            else
            {
                Pee = null;
            }



            if (Pee != null)
            {
                useString = useOnPee;// "{use} to Clean";
            }
            else
            {
                useString = string.Empty;
            }
        }

    }
}