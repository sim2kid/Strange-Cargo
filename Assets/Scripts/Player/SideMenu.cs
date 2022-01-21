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
            if (thisButton && lastButton != thisButton && !Utility.Toolbox.Instance.Pause.Paused)
                ToggleSideMenu();
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
            sideMenu.transform.localPosition = Vector3.zero;
            sideMenu.transform.localRotation = Quaternion.identity;
            sideMenu.transform.localScale = Vector3.one;
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