using Creature;
using PersistentData;
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
            }
            _finished = value;
        }
    }
    bool Loading;

    private void Awake()
    {
        _finished = true;
        Loading = false;
    }

    private void Start()
    {
        Loading = false;
        NormalOverlay.SetActive(true);
        Toolbox.Instance.Pause.SetPause(false);
        PauseMenu.SetActive(true);
        Cursor.visible = false;
        camera.Priority = -1000;
        End.Invoke();
        LoadingScene.SetActive(false);
        LoadingScreen.SetActive(false);

        FindObjectOfType<SaveManager>().OnLoad.AddListener(OnLoad);
        Invoke("LoadIsNeeded", 1f);
    }

    public void LoadIsNeeded() 
    {
        if (Report() < 1)
            OnLoad();
    }

    public float Report() 
    {
        List<CreatureController> creatures = Utility.Toolbox.Instance.CreatureList;
        float count = 0;
        foreach (CreatureController c in creatures)
            count += c.Report();
        if(creatures.Count > 0)
            count /= creatures.Count;
        else 
            count += 1;
        if (baseTime > 0)
        {
            count += Mathf.Clamp01(time / baseTime);
            count /= 2;
        }
        return count;
    }

    public void OnLoad() 
    {
        Loading = true;
        OnStart();
    }

    private void OnStart() 
    {
        Finished = false;
        Loading = true;
        Console.Log($"The scene and it's creatures are currently being loaded...");
        time = 0;
        saveCameraStyle = Camera.main.gameObject.GetComponent<Cinemachine.CinemachineBrain>().m_DefaultBlend;
        Camera.main.gameObject.GetComponent<Cinemachine.CinemachineBrain>().m_DefaultBlend.m_Style = Cinemachine.CinemachineBlendDefinition.Style.Cut;
        PauseMenu.SetActive(false);
        LoadingScreen.SetActive(true);
        NormalOverlay.SetActive(false);
        LoadingScene.SetActive(true);
        Toolbox.Instance.Pause.SetPause(true);
        Cursor.visible = true;
        bgm.Volume = 1;
        ProgrssBar.value = 0;

        if (Utility.Toolbox.Instance.CreatureList.Count > 0)
            baseTime = 0;

        camera.Priority = 1000;

        StartEvent.Invoke();
    }

    private void OnFinished() 
    {
        Loading = false;
        Console.Log($"Finished loading in with a time of {time.ToString("0.00")} seconds!");
        NormalOverlay.SetActive(true);
        Toolbox.Instance.Pause.SetPause(false);
        PauseMenu.SetActive(true);
        Cursor.visible = false;

        baseTime = 3; // No more extra waiting after first load

        camera.Priority = -1000;
        End.Invoke();

        //Destroy(LoadingScene);
        LoadingScene.SetActive(false);
        bgm.Volume = 1;
        

        LoadingScreen.SetActive(false);
        Invoke("SetCamToNormal", 1f);
    }

    private void SetCamToNormal() 
    {
        Camera.main.gameObject.GetComponent<Cinemachine.CinemachineBrain>().m_DefaultBlend = saveCameraStyle;
    }

    // Update is called once per frame
    void Update()
    {
        if (Loading)
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
                OnFinished();
            }
        }

    }
}
