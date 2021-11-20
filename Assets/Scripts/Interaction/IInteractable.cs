using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interaction
{
    public interface IInteractable
    {
        public GameObject GameObject { get; }
        public string InteractText { get; }
        public ClickState ClickState { get; }

        public void Enter();
        public void Hover();
        public void Exit();

        public void Down();
        public void Hold();
        public void Up();
        public void LateUp();
    }
}