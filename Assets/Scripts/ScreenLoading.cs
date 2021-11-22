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
        return count;
    }

    private void OnStart() 
    {
        LoadingScreen.SetActive(true);
        NormalOverlay.SetActive(false);
        Toolbox.Instance.Pause.SetPause(true);
        bgm.Volume = 1;
        ProgrssBar.value = 0;
        camera.Priority = 1000;
        StartEvent.Invoke();
    }

    private void OnFinished() 
    {
        LoadingScreen.SetActive(false);
        NormalOverlay.SetActive(true);
        Toolbox.Instance.Pause.SetPause(false);
        bgm.Volume = 1;

        camera.Priority = -1000;
        End.Invoke();

        LoadingScene.SetActive(false);

        this.enabled = false;
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
                ProgrssBar.value = Report();
            }
            if (Report() == 1 && !Finished)
            {
                Finished = true;
            }
        }

    }
}
