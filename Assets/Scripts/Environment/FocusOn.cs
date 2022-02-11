using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Utility;

namespace Environment
{
    public class FocusOn : MonoBehaviour
    {
        bool _inFocus = false;

        [SerializeField]
        Cinemachine.CinemachineVirtualCamera virtualCamera;

        private InputAction Back;

        public UnityEvent OnFocus;
        public UnityEvent OnUnFocus;

        public bool InFocus => _inFocus;

        void Start()
        {
            if (virtualCamera == null)
            {
                Console.LogWarning($"No virtual camera set in Focus On script on object {name}.");
            }
            else
            {
                virtualCamera.Priority = -1000;
            }
            PlayerInput input = FindObjectOfType<PlayerInput>();
            if (input == null) 
            {
                Console.LogWarning($"Could not find the Player Input in the scene!");
                return;
            }
            Back = input.actions["Back"];
        }

        private void Update()
        {
            if (Back.triggered) 
            {
                if (InFocus)
                {
                    UnFocus();
                }
            }
        }

        public void Focus() 
        {
            SetFocus(true);
        }

        public void UnFocus() 
        {
            SetFocus(false);
        }

        public void ToggleFocus() 
        {
            SetFocus(!InFocus);
        }

        private void onFocus() 
        {
            OnFocus.Invoke();
            if (virtualCamera != null)
            {
                virtualCamera.Priority = 1000;
            }
            FindObjectOfType<UI.UIManager>().OpenFocus();
            FindObjectOfType<Player.PlayerController>().Freeze();
        }

        private void onUnfocus() 
        {
            OnUnFocus.Invoke();
            if (virtualCamera != null)
            {
                virtualCamera.Priority = -1000;
            }
            FindObjectOfType<UI.UIManager>().OpenGameplay().SetCamToNormal();
            FindObjectOfType<UI.PauseMenu>().CancelPauseInput();
            FindObjectOfType<Player.PlayerController>().UnFreeze();
        }

        private void SetFocus(bool newFocus) 
        {
            if (newFocus != InFocus) 
            {
                _inFocus = newFocus;
                if (InFocus)
                {
                    onFocus();
                }
                else 
                {
                    onUnfocus();
                }
            }
        }
    }
}