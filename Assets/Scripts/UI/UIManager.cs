using Cinemachine;
using PersistentData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public ScreenLoading LoadingScreenManager;
        public PauseMenu PauseMenuManager;
        public GameObject HUD;
        public Menu currentMenu { get; private set; }


        private CinemachineBlendDefinition saveCameraStyle;

        // public MainMenu MainMenuManager;

        void Start()
        {
            OpenMainMenu();
            Invoke("LateStart", 1f);
        }

        void LateStart()
        {
            FindObjectOfType<SaveManager>().DestroyAllUnloadedCreatures();
        }

        public void CloseAllMenus()
        {
            saveCameraStyle = Camera.main.gameObject.GetComponent<Cinemachine.CinemachineBrain>().m_DefaultBlend;
            Camera.main.gameObject.GetComponent<Cinemachine.CinemachineBrain>().m_DefaultBlend.m_Style = Cinemachine.CinemachineBlendDefinition.Style.Cut;
            Invoke("SetCamToNormal", 2f);

            currentMenu = Menu.None;

            HUD.SetActive(false);
            LoadingScreenManager.CloseLoadingScreen();
            PauseMenuManager.DisablePauseMenu();
            PauseMenuManager.DisableMainMenu();
        }

        public void OpenMainMenu()
        {
            if (currentMenu == Menu.Main)
                return;
            Console.Log("Opening Main Menu");
            CloseAllMenus();
            currentMenu = Menu.Main;
            PauseMenuManager.EnableMainMenu();
            // Destroy all creatures that aren't loaded
            FindObjectOfType<SaveManager>().DestroyAllUnloadedCreatures();
        }

        public void OpenLoading()
        {
            if (currentMenu == Menu.Loading)
                return;
            Console.Log("Opening Loading Menu");
            CloseAllMenus();
            currentMenu = Menu.Loading;
            LoadingScreenManager.OpenLoadingScreen();
        }

        public void OpenGameplay()
        {
            if (currentMenu == Menu.Gameplay)
                return;
            Console.Log("Opening Gameplay Menu");
            CloseAllMenus();
            currentMenu = Menu.Gameplay;
            PauseMenuManager.OpenMenuOnPause = true;
            HUD.SetActive(true);
        }

        private void SetCamToNormal()
        {
            Camera.main.gameObject.GetComponent<Cinemachine.CinemachineBrain>().m_DefaultBlend = saveCameraStyle;
        }
    }

    public enum Menu
    {
        Loading,
        Main,
        Gameplay,
        None
    }
}