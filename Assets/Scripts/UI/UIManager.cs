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

        public UIManager CloseAllMenus()
        {
            Utility.Toolbox.Instance.Player.InputState = InputState.Default;
            saveCameraStyle = Camera.main.gameObject.GetComponent<Cinemachine.CinemachineBrain>().m_DefaultBlend;
            Camera.main.gameObject.GetComponent<Cinemachine.CinemachineBrain>().m_DefaultBlend.m_Style = Cinemachine.CinemachineBlendDefinition.Style.Cut;
            Invoke("SetCamToNormal", 2f);

            currentMenu = Menu.None;

            HUD.SetActive(false);
            LoadingScreenManager.CloseLoadingScreen();
            PauseMenuManager.DisablePauseMenu();
            PauseMenuManager.DisableMainMenu();
            return this;
        }

        public UIManager OpenMainMenu()
        {
            if (currentMenu == Menu.Main)
                return this;
            Console.Log("Opening Main Menu");
            CloseAllMenus();
            currentMenu = Menu.Main;
            PauseMenuManager.EnableMainMenu();
            // Destroy all creatures that aren't loaded
            FindObjectOfType<SaveManager>().DestroyAllUnloadedCreatures();
            return this;
        }

        public UIManager OpenLoading()
        {
            if (currentMenu == Menu.Loading)
                return this;
            Console.Log("Opening Loading Menu");
            CloseAllMenus();
            currentMenu = Menu.Loading;
            LoadingScreenManager.OpenLoadingScreen();
            return this;
        }

        public UIManager OpenGameplay()
        {
            if (currentMenu == Menu.Gameplay)
                return this;
            Console.Log("Opening Gameplay Menu");
            CloseAllMenus();
            currentMenu = Menu.Gameplay;
            PauseMenuManager.OpenMenuOnPause = true;
            HUD.SetActive(true);
            Utility.Toolbox.Instance.Player.InputState = InputState.Default;
            return this;
        }

        public UIManager OpenFocus() 
        {
            if (currentMenu == Menu.Focus)
                return this;
            Console.Log("Opening Focus UI");
            CloseAllMenus().SetCamToNormal();
            currentMenu = Menu.Focus;
            Utility.Toolbox.Instance.Player.InputState = InputState.UI;
            return this;
        }

        public UIManager SetCamToNormal()
        {
            Camera.main.gameObject.GetComponent<Cinemachine.CinemachineBrain>().m_DefaultBlend = saveCameraStyle;
            return this;
        }
    }

    public enum Menu
    {
        Loading,
        Main,
        Gameplay,
        Focus,
        None
    }
}