using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

namespace Animatics
{
    public class OpeningScene : MonoBehaviour
    {
        public UnityEvent OnFinish = new UnityEvent();
        public VideoClip video;

        void Start()
        {
            video = Resources.Load<VideoClip>("Video/StrangeCargoIntroAnimatic");

            Utility.Toolbox.Instance.Pause.SetPause(true);
            // play animatic
            GameObject.FindObjectOfType<UIManager>().OpenVideo(video).AddListener(() => 
            {
                Utility.Toolbox.Instance.Pause.SetPause(false);
                OnFinish.Invoke();
            });
        }
    }
}