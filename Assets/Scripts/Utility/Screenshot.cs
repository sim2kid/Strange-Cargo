using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

public class Screenshot : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction screenshot;

    void Start()
    {
        playerInput = GameObject.FindObjectOfType<PlayerInput>();
        screenshot = playerInput.actions["Screenshot"];

        
    }

    // Update is called once per frame
    void Update()
    {
        if (screenshot.triggered) 
        {
            SaveScreenshot();
        }
    }

    public void SaveScreenshot() 
    {
        string saveLoc = FileSaveLoc();
        ScreenCapture.CaptureScreenshot(saveLoc);
        Console.Log($"Screenshot has been saved at \"{saveLoc}\"");
    }

    private string FileSaveLoc() 
    {
        string p = Path.Combine(Application.persistentDataPath, "Screenshots");
        if (!Directory.Exists(p))
        { 
            Directory.CreateDirectory(p);
        }

        return Path.Combine(Application.persistentDataPath, "Screenshots", TimeStamp() + ".png");
    }
    private string TimeStamp() 
    {
        return System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
    }
}
