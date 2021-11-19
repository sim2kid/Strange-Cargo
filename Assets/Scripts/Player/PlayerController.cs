using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Movement;
using Interaction;

namespace Player
{
    [RequireComponent(typeof(MovementController))]
    [RequireComponent(typeof(HeadMovement))]
    [RequireComponent(typeof(InteractionController))]
    [RequireComponent(typeof(GlobalController))]
    [DisallowMultipleComponent]
    public class PlayerController : MonoBehaviour
    {
        [HideInInspector]
        public InteractionController Interaction;
        [HideInInspector]
        public HeadMovement HeadMovement;
        [HideInInspector]
        public MovementController Movement;
        [HideInInspector]
        public GlobalController GlobalInteraction;
        [HideInInspector]
        public Hand HandController;

        public GameObject Eyes;
        public GameObject Feet;
        public GameObject Hand;

        private void OnEnable()
        {
            Utility.Toolbox.Instance.Player = this;
        }

        private void Start()
        {
            Interaction = GetComponent<InteractionController>();
            if(Eyes != null)
                Interaction.Eyes = Eyes;
            HeadMovement = GetComponent<HeadMovement>();
            if (Eyes != null)
                HeadMovement.camera = Eyes;
            Movement = GetComponent<MovementController>();
            if (Feet != null)
                Movement.feet = Feet.transform;
            GlobalInteraction = GetComponent<GlobalController>();
            if (Hand != null)
                HandController = Hand.GetComponent<Hand>();
        }
    }
}