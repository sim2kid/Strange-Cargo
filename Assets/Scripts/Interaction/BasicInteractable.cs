using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interaction {
    public class BasicInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private string _hoverText;

        public GameObject GameObject { get => this.gameObject; }
        public string InteractText { get => _hoverText; }
        public ClickState ClickState { get; protected set; }

        protected virtual void Start()
        {
            ClickState = ClickState.None;
        }

        public virtual void Down()
        {
            ClickState = ClickState.Down;
        }

        public virtual void Enter()
        {
            ClickState = ClickState.Enter;
        }

        public virtual void Exit()
        {
            ClickState = ClickState.Exit;
        }

        public virtual void Hold()
        {
            ClickState = ClickState.Hold;
        }

        public virtual void Hover()
        {
            ClickState = ClickState.Hover;
        }

        public virtual void Up()
        {
            ClickState = ClickState.Up;
        }

        public virtual void LateUp()
        {
            
        }
    }
}