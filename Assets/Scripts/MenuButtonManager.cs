using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;

    public GameObject defaultMainButton;
    public GameObject defaultSettingsButton;
    // Start is called before the first frame update
    void Start()
    {
        ShowMain();
    }
    public void ShowMain()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(defaultMainButton);
    }
    public void ShowSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(defaultSettingsButton);
    }
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitToDesktop()
    {
        Utility.Toolbox.Instance.OnClosing.Invoke();
        Invoke("Terminate", 0.2f);
    }

    private void Terminate()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        return;
        #endif

        Application.Quit();
    }
}
