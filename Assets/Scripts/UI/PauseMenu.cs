using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Utility;

public class PauseMenu : MonoBehaviour
{
    public GameObject Menu;

    public PlayerInput playerInput;
    private InputAction Pause;

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

    public void ExitToMainMenu()
    {
        Toolbox.Instance.OnClosing.Invoke();
        Invoke("SceneChange", 1f);
    }

    public void ExitToDesktop()
    {
        Toolbox.Instance.OnClosing.Invoke();
        Invoke("Terminate", 1f);
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

    public void Options()
    {
        Console.Log("Options Menu has not been implemented yet.");
    }

    private void OnPause() 
    {
        Menu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnUnPause() 
    {
        Menu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
