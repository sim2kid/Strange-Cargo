using Cinemachine;
using PersistentData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public ScreenLoading LoadingScreenManager;
        public PauseMenu PauseMenuManager;
        public GameObject HUD;
        public Menu currentMenu { get; private set; }


        private CinemachineBlendDefinition defaultCameraStyle = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 1f);

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

        delegate void RunThis();
        List<RunThis> toRun = new List<RunThis>();
        private void LateUpdate()
        {
            foreach (var func in toRun) 
            {
                func();
            }
            toRun.Clear();
        }

        private void RunNextFrame(RunThis function) 
        {
            toRun.Add(function);
        }

        public UIManager CloseAllMenus()
        {
            Utility.Toolbox.Instance.Player.InputState = InputState.Default;
            Camera.main.gameObject.GetComponent<Cinemachine.CinemachineBrain>().m_DefaultBlend.m_Style = Cinemachine.CinemachineBlendDefinition.Style.Cut;
            SetCamToNormal();

            currentMenu = Menu.None;
            Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("Player"));

            HUD.SetActive(false);
            LoadingScreenManager.CloseLoadingScreen();
            PauseMenuManager.DisablePauseMenu();
            PauseMenuManager.DisableMainMenu();
            return this;
        }

        public UIManager NextState(Menu menu) 
        {
            switch (menu) 
            {
                case Menu.Main:
                    return OpenMainMenu();
                case Menu.Loading:
                    return OpenLoading();
                case Menu.Gameplay:
                    return OpenGameplay();
                case Menu.Focus:
                    return OpenFocus();
                default:
                    return OpenGameplay();
            }
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
            Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("Player");
            return this;
        }

        public UnityEvent OpenVideo(UnityEngine.Video.VideoClip videoClip) 
        {
            var lastState = currentMenu;
            if (currentMenu == Menu.Video)
                return null;
            Console.Log("Opening Video Player.");
            CloseAllMenus();
            currentMenu = Menu.Video;
            Utility.Toolbox.Instance.Pause.SetPause(true);
            UnityEvent onFinish = new UnityEvent();

            var bgm = GameObject.FindObjectOfType<Sound.BackgroundMusic>();
            bgm.PauseMusic();

            PauseMenuManager.PlayVideo(videoClip).AddListener(() => 
            {
                Utility.Toolbox.Instance.Pause.SetPause(false);
                bgm.UnpauseMusic();
                onFinish.Invoke();
                NextState(lastState);
            });
            return onFinish;
        }

        public UIManager SetCamToNormal()
        {
            RunNextFrame(() => {
                Camera.main.gameObject.GetComponent<Cinemachine.CinemachineBrain>().m_DefaultBlend = defaultCameraStyle;
            });
            return this;
        }
    }

    public enum Menu
    {
        Loading,
        Main,
        Gameplay,
        Focus,
        Video,
        None
    }
}