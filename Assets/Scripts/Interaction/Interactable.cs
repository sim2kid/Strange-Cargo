using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Interaction
{
    public class Interactable : MonoBehaviour, IInteractable
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

        public ClickState ClickState { get; protected set; }

        public void Start()
        {
            ClickState = ClickState.None;
        }

        public void Down()
        {
            ClickState = ClickState.Down;
            OnDown.Invoke();
        }

        public void Enter()
        {
            ClickState = ClickState.Enter;
            OnEnter.Invoke();
        }

        public void Exit()
        {
            ClickState = ClickState.Exit;
            OnExit.Invoke();
        }

        public void Hold()
        {
            ClickState = ClickState.Hold;
            OnHold.Invoke();
        }

        public void Hover()
        {
            ClickState = ClickState.Hover;
            OnHover.Invoke();
        }

        public void Up()
        {
            ClickState = ClickState.Up;
            OnUp.Invoke();
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