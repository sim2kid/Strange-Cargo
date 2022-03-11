using PersistentData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Utility;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject Menu;
        public GameObject pauseMenu;
        public GameObject OptionsMenu;
        public GameObject ExitMenu;
        public GameObject LoadMenu;
        public GameObject MainMenu;

        public PlayerInput playerInput;
        private InputAction Pause;

        public GameObject pauseDefault;
        public GameObject optionsDefault;
        public GameObject exitDefault;
        public GameObject loadDefault;
        public GameObject mainDefault;

        public Cinemachine.CinemachineVirtualCamera MenuCamera;

        public bool OpenMenuOnPause;
        public bool UseMainMenu;

        private float IgnorePause = 0;

        private void OnEnable()
        {
            OpenMenuOnPause = false;
            UseMainMenu = false;

            if (Toolbox.Instance.Pause == null)
                return;

            Toolbox.Instance.Pause.OnPause.AddListener(OnPause);
            Toolbox.Instance.Pause.OnUnPause.AddListener(OnUnPause);

            if (Toolbox.Instance.Pause.Paused)
                OnPause();
        }

        private void OnDisable()
        {
            if (Toolbox.Instance.Pause == null)
                return;

            Toolbox.Instance.Pause.OnPause.RemoveListener(OnPause);
            Toolbox.Instance.Pause.OnUnPause.RemoveListener(OnUnPause);
        }

        private void Start()
        {
            if (playerInput == null)
                playerInput = FindObjectOfType<PlayerInput>();

            if (playerInput == null)
            {
                Console.LogError("Could not find player input in scene for the pause menu. Deactivating it.");
                gameObject.SetActive(false);
                return;
            }

            Toolbox.Instance.Pause.OnPause.AddListener(OnPause);
            Toolbox.Instance.Pause.OnUnPause.AddListener(OnUnPause);

            IgnorePause = 0;

            Pause = playerInput.actions["Pause"];
        }

        private void Update()
        {
            if (OpenMenuOnPause && Pause.triggered && IgnorePause <= 0)
            {
                Toolbox.Instance.Pause.SetPause(!Toolbox.Instance.Pause.Paused);
            }
            if (IgnorePause > 0) 
            {
                IgnorePause -= Time.deltaTime;
            }
        }

        public void CancelPauseInput()
        {
            IgnorePause = 0.1f;
        }

        public void Resume()
        {
            Toolbox.Instance.Pause.SetPause(false);
        }

        public void SaveAndExit()
        {
            Save();
            ExitToMainMenu();
        }

        public void Save()
        {
            var man = FindObjectOfType<SaveManager>();
            if (man == null)
            {
                Console.LogError("Could not save the game. Save manager is not in scene.");
                return;
            }
            man.Save();
        }

        public void Load()
        {
            TurnOffAllMenus();
            LoadMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(loadDefault);
        }

        public void ExitToMainMenu()
        {
            FindObjectOfType<UIManager>().OpenMainMenu();
        }

        public void ExitToDesktop()
        {
            Toolbox.Instance.OnClosing.Invoke();
            Invoke("Terminate", 0.2f);
        }

        private void Terminate()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            return;
#endif
            if (Application.platform == RuntimePlatform.WebGLPlayer) 
            {
                Application.OpenURL("#");
                return;
            }

            Application.Quit();
        }

        public void Exit()
        {
            TurnOffAllMenus();
            ExitMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(exitDefault);
        }

        public void Options()
        {
            Console.Log("Options Menu has not been implemented yet.");
            TurnOffAllMenus();
            OptionsMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(optionsDefault);
        }

        public void BackToPauseMenu()
        {
            TurnOffAllMenus();
            if (UseMainMenu)
            {
                MainMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(mainDefault);
            }
            else
            {
                pauseMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(pauseDefault);
            }
        }

        private void OnPause()
        {
            if (OpenMenuOnPause)
            {
                Menu.SetActive(true);
                BackToPauseMenu();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Utility.Toolbox.Instance.Player.InputState = InputState.UI;
                UIHook.PlaySound(SFX.OPEN);
            }
        }

        public void SaveToResources()
        {
            if (!Application.isEditor)
            {
                Console.LogError("You can only set the default level in the editor.");
                return;
            }
            var man = FindObjectOfType<SaveManager>();
            if (man == null)
            {
                Console.LogError("Could not save the game. Save manager is not in scene.");
                return;
            }
            man.SaveAsDefault();
            Console.Log("New Default Saved!");
        }

        private void OnUnPause()
        {
            if (OpenMenuOnPause)
            {
                if(Menu.activeSelf)
                    UIHook.PlaySound(SFX.CLOSE);
                Menu.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Utility.Toolbox.Instance.Player.InputState = InputState.Default;
                
            }
        }

        public void DisablePauseMenu()
        {
            OpenMenuOnPause = false;
            Menu.SetActive(false);
        }

        public void EnableMainMenu()
        {
            Menu.SetActive(true);
            Utility.Toolbox.Instance.Player.InputState = InputState.UI;
            if (MenuCamera != null)
                MenuCamera.Priority = 1000;
            UseMainMenu = true;
            BackToPauseMenu();
            Resume();
            FindObjectOfType<Player.PlayerController>().Disable();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        public void DisableMainMenu()
        {
            if (MenuCamera != null)
                MenuCamera.Priority = -1000;
            UseMainMenu = false;
            Menu.SetActive(false);
            FindObjectOfType<Player.PlayerController>().Enable();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void TurnOffAllMenus()
        {
            LoadMenu.SetActive(false);
            MainMenu.SetActive(false);
            pauseMenu.SetActive(false);
            OptionsMenu.SetActive(false);
            ExitMenu.SetActive(false);
        }
    }
}