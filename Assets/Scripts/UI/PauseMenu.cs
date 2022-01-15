using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Utility;

public class PauseMenu : MonoBehaviour
{
    public GameObject Menu;
    public GameObject pauseMenu;
    public GameObject OptionsMenu;
    public GameObject ExitMenu;
    public GameObject LoadMenu;
    public GameObject SaveMenu;

    public PlayerInput playerInput;
    private InputAction Pause;

    public GameObject pauseDefault;
    public GameObject optionsDefault;
    public GameObject exitDefault;
    public GameObject saveDefault;
    public GameObject loadDefault;

    private bool lastPause = false;
    private void OnEnable()
    {
        Toolbox.Instance.Pause.OnPause.AddListener(OnPause);
        Toolbox.Instance.Pause.OnUnPause.AddListener(OnUnPause);

        Menu.SetActive(Toolbox.Instance.Pause.Paused);
    }

    private void OnDisable()
    {
        Toolbox.Instance.Pause.OnPause.RemoveListener(OnPause);
        Toolbox.Instance.Pause.OnUnPause.RemoveListener(OnUnPause);
    }

    private void Start()
    {
        if (playerInput == null)
            playerInput = FindObjectOfType<PlayerInput>();

        if (playerInput == null) {
            Console.LogError("Could not find player input in scene for the pause menu. Deactivating it.");
            gameObject.SetActive(false);
            return;
        }

        Pause = playerInput.actions["Pause"];
        lastPause = Pause.ReadValue<float>() == 1;
    }

    private void Update()
    {

        bool currentPause = Pause.ReadValue<float>() == 1;
        if (currentPause && lastPause != currentPause)
            Toolbox.Instance.Pause.SetPause(!Toolbox.Instance.Pause.Paused);

        lastPause = currentPause;
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
        TurnOffAllMenus();
        SaveMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(saveDefault);
        Console.Log("Saving has not been implemnted yet.");
    }

    public void Load() 
    {
        TurnOffAllMenus();
        LoadMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(loadDefault);
        Console.Log("Loading has not been implemnted yet.");
    }

    public void ExitToMainMenu()
    {
        Toolbox.Instance.OnClosing.Invoke();
        Invoke("SceneChange", 0.2f);
    }

    public void ExitToDesktop()
    {
        Toolbox.Instance.OnClosing.Invoke();
        Invoke("Terminate", 0.2f);
    }

    private void SceneChange() 
    {
        SceneManager.LoadScene(0);
    }

    private void Terminate() 
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        return;
        #endif

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
        pauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(pauseDefault);
    }

    private void OnPause() 
    {
        Menu.SetActive(true);
        BackToPauseMenu();
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnUnPause() 
    {
        Menu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void TurnOffAllMenus() 
    {
        LoadMenu.SetActive(false);
        SaveMenu.SetActive(false);
        pauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        ExitMenu.SetActive(false);
    }
}
