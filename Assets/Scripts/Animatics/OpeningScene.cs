using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Video;

namespace Animatics
{
    public class OpeningScene : MonoBehaviour
    {
        public UnityEvent OnFinish = new UnityEvent();
        public VideoClip video;
        bool isPlaying;
        bool anyKey;

        void Start()
        {
            video = Resources.Load<VideoClip>("Video/StrangeCargoIntroAnimatic");
            var bgm = GameObject.FindObjectOfType<Sound.BackgroundMusic>();

            Utility.Toolbox.Instance.Pause.SetPause(true);
            bgm.PauseMusic();
            // play animatic
            isPlaying = true;
            GameObject.FindObjectOfType<UIManager>().OpenVideo(video)?.AddListener(() => 
            {
                isPlaying = false;
                Utility.Toolbox.Instance.Pause.SetPause(false);
                bgm.UnpauseMusic();
                OnFinish.Invoke();
            });

            InputSystem.onEvent +=
            (eventPtr, device) =>
            {
                if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
                    return;
                var controls = device.allControls;
                var buttonPressPoint = InputSystem.settings.defaultButtonPressPoint;
                for (var i = 0; i < controls.Count; ++i)
                {
                    var control = controls[i] as ButtonControl;
                    if (control == null || control.synthetic || control.noisy)
                        continue;
                    if (control.ReadValueFromEvent(eventPtr, out var value) && value >= buttonPressPoint)
                    {
                        anyKey = true;
                        break;
                    }
                }
            };
        }

        private void Update()
        {
            if (isPlaying && anyKey) 
            {
                isPlaying = false;
                GameObject.FindObjectOfType<PauseMenu>().StopVideo();
            }
        }
    }
}