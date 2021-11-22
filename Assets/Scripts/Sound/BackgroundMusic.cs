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
            ap.Volume *= 0.25f;
        }

        void OnUnPause() 
        {
            ap.Volume /= 0.25f;
        }

        // Update is called once per frame
        void Update()
        {
            LoopSound loop = (LoopSound)ap.Sound;
            float timeChange = 24f / trackCount;
            int expectedIndex = (int)Mathf.Floor(time.CurrentTime / timeChange);
            if (expectedIndex != index) 
            {
                loop.SetIndex(expectedIndex);
                index = expectedIndex;
                ap.Stop();
            }
        }
    }
}