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
        public UnityEvent UseEvent;
        public UnityEvent PrimaryEvent;
        public UnityEvent SecondaryEvent;

        private PlayerInput playerInput;
        private InputAction use;
        private InputAction primary;
        private InputAction second;

        private bool calledUse;
        private bool calledPrimary;
        private bool calledSecond;


        private void Awake()
        {
            if(UseEvent == null)
                UseEvent = new UnityEvent();
            if(PrimaryEvent == null)
                PrimaryEvent = new UnityEvent();
            if(SecondaryEvent == null)
                SecondaryEvent = new UnityEvent();
        }

        void Start()
        {
            playerInput = GetComponent<PlayerInput>();
            use = playerInput.actions["Use"];
            primary = playerInput.actions["Primary"];
            second = playerInput.actions["Secondary"];
            calledUse = false;
            calledSecond = false;
            calledPrimary = false;
        }

        // Update is called once per frame
        void Update()
        {
            bool uDown = use.ReadValue<float>() == 1;
            bool pDown = primary.ReadValue<float>() == 1;
            bool sDown = second.ReadValue<float>() == 1;

            if (uDown && uDown != calledUse)
            {
                UseEvent.Invoke();
            }
            if (sDown && sDown != calledSecond)
            {
                SecondaryEvent.Invoke();
            }
            if (pDown && pDown != calledPrimary)
            {
                PrimaryEvent.Invoke();
            }
            calledPrimary = pDown;
            calledUse = uDown;
            calledSecond = sDown;
        }
    }
}