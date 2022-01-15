using Creature;
using Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utility;

public class ScreenLoading : MonoBehaviour, IProgress
{
    public GameObject LoadingScreen;
    public GameObject NormalOverlay;
    public Slider ProgrssBar;
    public Cinemachine.CinemachineVirtualCamera camera;
    public GameObject LoadingScene;
    public AudioPlayer bgm;
    public GameObject PauseMenu;

    Cinemachine.CinemachineBlendDefinition saveCameraStyle;
    float time = 0;
    float baseTime = 23;

    public UnityEvent StartEvent;
    public UnityEvent End;

    private bool _finished;
    public bool Finished 
    {
        get 
        {
            return _finished;
        }
        private set 
        {
            if (value != _finished) 
            {
                if (value)
                {
                    OnFinished();
                }
                else 
                {
                    OnStart();
                }
            }
            _finished = value;
        }
    }

    private void Awake()
    {
        _finished = true;
    }

    public float Report() 
    {
        List<CreatureController> creatures = Utility.Toolbox.Instance.CreatureList;
        float count = 0;
        foreach (CreatureController c in creatures)
            count += c.Report();
        count /= creatures.Count;
        count += Mathf.Clamp01(time/baseTime);
        count /= 2;
        return count;
    }

    private void OnStart() 
    {
        Console.Log($"The scene and it's creatures are currently being loaded...");
        PauseMenu.SetActive(false);
        LoadingScreen.SetActive(true);
        NormalOverlay.SetActive(false);
        Toolbox.Instance.Pause.SetPause(true);
        Cursor.visible = false;
        bgm.Volume = 1;
        ProgrssBar.value = 0;
        camera.Priority = 1000;
        saveCameraStyle = Camera.main.gameObject.GetComponent<Cinemachine.CinemachineBrain>().m_DefaultBlend;
        Camera.main.gameObject.GetComponent<Cinemachine.CinemachineBrain>().m_DefaultBlend.m_Style = Cinemachine.CinemachineBlendDefinition.Style.Cut;
        StartEvent.Invoke();
    }

    private void OnFinished() 
    {
        Console.Log($"Finished loading in with a time of {time.ToString("0.00")} seconds!");
        NormalOverlay.SetActive(true);
        Toolbox.Instance.Pause.SetPause(false);
        PauseMenu.SetActive(true);

        camera.Priority = -1000;
        End.Invoke();

        //Destroy(LoadingScene);
        LoadingScene.SetActive(false);
        bgm.Volume = 1;
        

        LoadingScreen.SetActive(false);
        Invoke("SelfDestruct", 0.01f);
    }

    private void SelfDestruct() 
    {
        Camera.main.gameObject.GetComponent<Cinemachine.CinemachineBrain>().m_DefaultBlend = saveCameraStyle;
        //Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Utility.Toolbox.Instance.CreatureList.Count > 0)
        {
            if (Report() < 1 && Finished)
            {
                Finished = false;
            }
            if (!Finished && Report() < 1)
            {
                time += Time.deltaTime;
                bgm.Volume = 1;
                ProgrssBar.value = Report();
            }
            if (Report() == 1 && !Finished)
            {
                Finished = true;
            }
        }

    }
}
