using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Sound
{
    [RequireComponent(typeof(AudioPlayer))]
    public class BackgroundMusic : MonoBehaviour
    {
        AudioPlayer ap;

        [SerializeField]
        LoopSound Sound;

        [SerializeField]
        List<float> TimesToSwitchTracks;
        int listIndex = 0;

        TimeController time;

        int index = 0;
        int trackCount { get => getTrackCount();  }

        private int getTrackCount() 
        {
            LoopSound loop = (LoopSound)ap.Sound;
            if (loop == null)
            {
                Debug.LogError("Could not convert the Loop-Sound for the Background Music");
                return 0;
            }
            return loop.Count();
        }

        // Start is called before the first frame update
        void Start()
        {
            ap = GetComponent<AudioPlayer>();
            ap.Sound = Sound;
            ap.Sound.Loop = true;
            ap.enabled = true;
            ap.Sound.LoadAudio();

            time = Toolbox.Instance.TimeController;
            Utility.Toolbox.Instance.Pause.OnPause.AddListener(OnPause);
            Utility.Toolbox.Instance.Pause.OnUnPause.AddListener(OnUnPause);
        }

        void OnPause() 
        {
            ap.Volume *= 0.50f;
        }

        void OnUnPause() 
        {
            ap.Volume /= 0.50f;
        }


        float lastTime = 0;

        // Update is called once per frame
        void Update()
        {
            if (lastTime > time.CurrentTime) 
            {
                if (TimesToSwitchTracks[listIndex] > 24) 
                {
                    TimesToSwitchTracks[listIndex] -= 24;
                }
            }

            if (time.CurrentTime >= TimesToSwitchTracks[listIndex]) 
            {
                NextTrack();

                listIndex++;
                if (time.CurrentTime >= TimesToSwitchTracks[listIndex]) 
                {
                    TimesToSwitchTracks[listIndex] += 24;
                }
            }

            lastTime = time.CurrentTime;
        }

        void NextTrack()
        {
            LoopSound loop = (LoopSound)ap.Sound;

            index++;
            if (index >= trackCount)
                index = 0;

            loop.SetIndex(index);
            ap.Stop();
        }
    }
}