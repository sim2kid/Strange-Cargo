using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;


public class EoDUIManager : MonoBehaviour
{
    //UI
    public GameObject EoD_UI;

    Pause pause;

    public void SaveQuit()
    {
        // Saves and Exits to Main Menu
        FindObjectOfType<UI.PauseMenu>().SaveAndExit();
    }

    public void Continue()
    {
        // Next Day
        var time = FindObjectOfType<TimeController>();
        time.SetTime(6); // in hours

        // Doesnt skip a day if it's between midnight and 6am
        if (time.GetTime() > 6)
        {
            FindObjectOfType<DateController>().IncrementDate(); // Next Day
        }

        // Close Menu
        EoD_UI.SetActive(false);
        // unpause and continue
        pause.SetPause(false);
    }

    public void OpenEoD_UI()
    {
        // pause
        pause.SetPause(true);
        // open menu
        EoD_UI.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        EoD_UI.SetActive(false);
        pause = Toolbox.Instance.Pause;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
