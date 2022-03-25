using PersistentData.Saving;
using PersistentData.Component;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

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
        public float toggleTime = 0.2f;

        private float timePassed;
        private Vector3 initialPosition;
        private Quaternion initialRotation;
        private Vector3 initialScale;
        private UnityEvent LerpEvent;

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
            LerpEvent = new UnityEvent();
            initialRotation = Quaternion.identity;
            initialScale = Vector3.one; 
            initialPosition = Vector3.zero;
            timePassed = 0;
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
                LerpEvent.Invoke();
            }
            lastButton = thisButton;
        }

        public void SetSideMenu(bool isActive) 
        {
            if (isActive)
            {
                IsActive = true;
                sideMenu.transform.parent = activeLocation;
                sideMenu.layer = 8;
            }
            else
            {
                IsActive = false;
                sideMenu.transform.parent = inactiveLocation;
                sideMenu.layer = 0;
            }
            // Setup starting info
            initialPosition = sideMenu.transform.localPosition;
            initialRotation = sideMenu.transform.localRotation;
            initialScale = sideMenu.transform.localScale;
            timePassed = 0;
            LerpEvent.AddListener(HandleClipboardPosition);
        }

        private void HandleClipboardPosition()
        {
            // Figure out time
            float fracComplete = timePassed / toggleTime;
            timePassed += Time.deltaTime;
            // If event is over, unregister it
            if (timePassed >= toggleTime)
            { 
                LerpEvent.RemoveAllListeners();
            }

            // Run Slerp localy
            sideMenu.transform.localPosition = Vector3.Slerp(initialPosition, Vector3.zero, fracComplete);
            sideMenu.transform.localRotation = Quaternion.Slerp(initialRotation, Quaternion.identity, fracComplete);
            sideMenu.transform.localScale = Vector3.Slerp(initialScale, Vector3.one, fracComplete);
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