using PersistentData.Saving;
using PersistentData.Component;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Player
{

    [RequireComponent(typeof(PlayerInput))]
    public class SideMenu : MonoBehaviour, ISaveable
    {
        PlayerInput input;
        InputAction sidemenu;
        [SerializeField]
        Transform activeLocation;
        [SerializeField]
        Transform inactiveLocation;
        [SerializeField]
        GameObject sideMenu;
        [Tooltip("The time it takes the clipboard to move between the active and inactive locations.")]
        public float toggleTime = 0.1875f;
        [Tooltip("The time it which the player toggled on or off.")]
        private float toggleStartTime;

        public IsActiveData data;
        public bool IsActive { get => data.IsActive; set => data.IsActive = value; }
        public ISaveData saveData { get => data; set => data = (IsActiveData)value; }
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(data._guid))
            {
                data._guid = System.Guid.NewGuid().ToString();
            }
        }

        void Start()
        {
            SetSideMenu(false);
            input = GetComponent<PlayerInput>();
            sidemenu = input.actions["SideMenu"];
        }


        bool lastButton = false;
        void Update()
        {
            bool thisButton = sidemenu.ReadValue<float>() == 1;
            if (!Utility.Toolbox.Instance.Pause.Paused && // Run only if not paused
                (Utility.Toolbox.Instance.Player.InputState == InputState.Default || IsActive)) // Run only if in gameplay or if the menu is open
            {
                if (thisButton && lastButton != thisButton) // Run if button is pressed and it's a new change
                {
                    ToggleSideMenu();
                }
                HandleClipboardPosition();
            }
            lastButton = thisButton;
        }

        public void SetSideMenu(bool isActive) 
        {
            if (isActive)
            {
                IsActive = true;
                sideMenu.layer = 8;
            }
            else
            {
                IsActive = false;
                sideMenu.layer = 0;
            }
        }

        private void HandleClipboardPosition()
        {
            float fracComplete = (Time.time - toggleStartTime) / toggleTime;
            if (IsActive)
            {
                sideMenu.transform.localPosition = Vector3.Slerp(inactiveLocation.localPosition, activeLocation.localPosition, fracComplete);
            }
            else
            {
                sideMenu.transform.localPosition = Vector3.Slerp(activeLocation.localPosition, inactiveLocation.localPosition, fracComplete);
            }
            sideMenu.transform.localRotation = activeLocation.localRotation;
            sideMenu.transform.localScale = Vector3.one;
        }

        void OnSideMenu()
        {
            toggleStartTime = Time.time;
        }

        public void ToggleSideMenu() 
        {
            SetSideMenu(!IsActive);
        }

        public void PreSerialization()
        {
            return;
        }

        public void PreDeserialization()
        {
            return;
        }

        public void PostDeserialization()
        {
            SetSideMenu(IsActive);
            return;
        }
    }
}