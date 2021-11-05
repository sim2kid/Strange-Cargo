using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Interaction
{
    [RequireComponent(typeof(PlayerInput))]
    [DisallowMultipleComponent]
    public class GlobalController : MonoBehaviour
    {
        [SerializeField]
        public UnityEvent PrimaryEvent;
        public UnityEvent SecondaryEvent;

        private PlayerInput playerInput;
        private InputAction interact;
        private InputAction second;

        private bool calledInteract;
        private bool calledSecond;

        void Start()
        {
            playerInput = GetComponent<PlayerInput>();
            interact = playerInput.actions["Interact"];
            second = playerInput.actions["Secondary"];
            calledInteract = false;
            calledSecond = false;
        }

        // Update is called once per frame
        void Update()
        {
            bool pDown = interact.ReadValue<float>() == 1;
            bool sDown = second.ReadValue<float>() == 1;

            if (pDown && pDown != calledInteract)
            {
                PrimaryEvent.Invoke();
            }
            if (sDown && sDown != calledSecond)
            {
                SecondaryEvent.Invoke();
            }
            calledInteract = pDown;
            calledSecond = sDown;
        }
    }
}