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
        public UnityEvent UseEvent;
        public UnityEvent Mod1UseEvent;
        public UnityEvent ThrowEvent;
        public UnityEvent DropEvent;
        [System.Obsolete("This is a poorly defined event and will be removed in the future.")]
        public UnityEvent SecondaryEvent;

        public bool Mod1Active => Modifier1.ReadValue<float>() == 1;

        private PlayerInput playerInput;
        private InputAction Throw;
        private InputAction Use;
        private InputAction Drop;
        [System.Obsolete("This is a poorly defined action and will be removed in the future.")]
        private InputAction second;
        private InputAction Modifier1;

        private void Awake()
        {
            if(UseEvent == null)
                UseEvent = new UnityEvent();
            if(DropEvent == null)
                DropEvent = new UnityEvent();
            if(SecondaryEvent == null)
                SecondaryEvent = new UnityEvent();
        }

        void Start()
        {
            playerInput = GetComponent<PlayerInput>();
            Use = playerInput.actions["Use"];
            Throw = playerInput.actions["Throw"];
            Drop = playerInput.actions["Drop"];
            second = playerInput.actions["Secondary"];
            Modifier1 = playerInput.actions["Modifier1"];
        }

        // Update is called once per frame
        void Update()
        {
            if (Use.triggered)
            {
                if (Mod1Active)
                {
                    Mod1UseEvent.Invoke();
                }
                else
                {
                    UseEvent.Invoke();
                }
            }
            if(Throw.triggered)
            {
                ThrowEvent.Invoke();
            }
            if (second.triggered)
            {
                SecondaryEvent.Invoke();
            }
            if (Drop.triggered)
            {
                DropEvent.Invoke();
            }
        }
    }
}