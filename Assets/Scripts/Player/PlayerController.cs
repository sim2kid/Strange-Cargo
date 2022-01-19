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
        public GameObject PlayerModel;
        public Camera OverlayCamera;

        private void Awake()
        {
            Utility.Toolbox.Instance.Player = this;
        }

        public void Disable() 
        {
            if (!Utility.Toolbox.Instance.Pause.Paused)
                OnPause();
            PlayerModel.SetActive(false);
        }

        public void Enable() 
        {
            if(!Utility.Toolbox.Instance.Pause.Paused)
                OnUnPause();
            PlayerModel.SetActive(true);    
        }

        private void OnPause() 
        {
            GlobalInteraction.enabled = false;
            HandController.enabled = false;
            HeadMovement.enabled = false;
            Interaction.enabled = false;
            Movement.enabled = false;
            OverlayCamera.enabled = false;
        }
        private void OnUnPause()
        {
            GlobalInteraction.enabled = true;
            HandController.enabled = true;
            HeadMovement.enabled = true;
            Interaction.enabled = true;
            Movement.enabled = true;
            OverlayCamera.enabled = true;
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

            if (Utility.Toolbox.Instance.Pause == null)
                Console.LogError("Missing Pause Component in Scene");

            Utility.Toolbox.Instance.Pause.OnPause.AddListener(OnPause);
            Utility.Toolbox.Instance.Pause.OnUnPause.AddListener(OnUnPause);
        }
    }
}