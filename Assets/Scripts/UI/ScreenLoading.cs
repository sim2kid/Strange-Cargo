using Creature;
using PersistentData;
using Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utility;

namespace UI
{
    public class ScreenLoading : MonoBehaviour, IProgress
    {
        public GameObject LoadingScreen;
        public Slider ProgrssBar;
        public Cinemachine.CinemachineVirtualCamera camera;
        public GameObject LoadingScene;
        UIManager UIManager;

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
            CloseLoadingScreen();

            UIManager = FindObjectOfType<UIManager>();
            FindObjectOfType<SaveManager>().OnLoad.AddListener(OnLoad);
            Invoke("LoadIsNeeded", 1f);
        }

        public void CloseLoadingScreen()
        {
            Loading = false;
            camera.Priority = -1000;
            LoadingScene.SetActive(false);
            LoadingScreen.SetActive(false);
            
        }

        public void OpenLoadingScreen()
        {
            camera.Priority = 1000;
            Utility.Toolbox.Instance.Player.InputState = InputState.UI;
            LoadingScene.SetActive(true);
            LoadingScreen.SetActive(true);
        }

        public void LoadIsNeeded()
        {
            if (Report() < 1 && UIManager.currentMenu == Menu.Gameplay)
                OnLoad();
        }

        public float Report()
        {
            List<CreatureController> creatures = Utility.Toolbox.Instance.CreatureList;
            float added = 0;
            int count = 0;

            // Creatures
            foreach (CreatureController c in creatures)
            {
                added += c.Report();
                count++;
            }

            // Timer
            if (baseTime > 0)
            {
                added += Mathf.Clamp01(time / baseTime);
                count++;
            }

            return added/count;
        }

        public void OnLoad()
        {
            UIManager.OpenLoading();
            OnStart();
        }

        private void OnStart()
        {
            Finished = false;
            Loading = true;
            Console.Log($"The scene and it's creatures are currently being loaded...");
            time = 0;
            UIManager.OpenLoading();
            Toolbox.Instance.Pause.SetPause(true);
            Cursor.visible = true;
            ProgrssBar.value = 0;

            StartEvent.Invoke();
        }

        private void OnFinished()
        {
            Loading = false;
            Console.Log($"Finished loading in with a time of {time.ToString("0.00")} seconds!");
            UIManager.OpenGameplay();
            Toolbox.Instance.Pause.SetPause(false);
            Cursor.visible = false;
            baseTime = 5; // No more extra waiting after first load
            End.Invoke();
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
                    //bgm.Volume = 1;
                    ProgrssBar.value = Report();
                }
                if (Report() == 1 && !Finished)
                {
                    Finished = true;
                    OnFinished();
                }
            }
            else if (UIManager.currentMenu == Menu.Loading)
            {
                UIManager.OpenGameplay();
            }
        }
    }
}