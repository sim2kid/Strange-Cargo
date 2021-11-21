using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Interaction
{
    public class Interactable : BasicInteractable
    {
        [Tooltip("Interaction Events")]
        [SerializeField]
        private Events events;

        public UnityEvent OnDown { get => events.OnDown; }
        public UnityEvent OnEnter { get => events.OnEnter; }
        public UnityEvent OnExit { get => events.OnExit; }
        public UnityEvent OnHold { get => events.OnHold; }
        public UnityEvent OnHover { get => events.OnHover; }
        public UnityEvent OnUp { get => events.OnUp; }

        public override void Down()
        {
            OnDown.Invoke();
            base.Down();
        }

        public override void Enter()
        {
            OnEnter.Invoke();
            base.Enter();
        }

        public override void Exit()
        {
            OnExit.Invoke();
            base.Exit();
        }

        public override void Hold()
        {
            OnHold.Invoke();
            base.Hold();
        }

        public override void Hover()
        {
            OnHover.Invoke();
            base.Hover();
        }

        public override void Up()
        {
            OnUp.Invoke();
            base.Up();
        }
    }

    [Serializable]
    class Events 
    {
        public UnityEvent OnEnter;
        public UnityEvent OnHover;
        public UnityEvent OnExit;
        
        public UnityEvent OnDown;
        public UnityEvent OnHold;
        public UnityEvent OnUp;
    }
}