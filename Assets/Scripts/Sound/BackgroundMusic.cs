using Sound.Player;
using Sound.Structure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound
{
    [RequireComponent(typeof(AudioPlayer))]
    public class BackgroundMusic : MonoBehaviour
    {
        [SerializeField]
        List<TrackTime> _tracks;
        private AudioPlayer ap;
        private Utility.TimeController time;
        private SwitchContainer switchContainer;
        private int lastTrack;

        void Start()
        {
            lastTrack = -1;
            ap = GetComponent<AudioPlayer>();
            time = FindObjectOfType<Utility.TimeController>();
            ISound container = ap.Container;
            if (container is SwitchContainer)
            {
                switchContainer = (SwitchContainer)container;
                ap.OnPlayEnd.AddListener(PlayMusic);
            }
            else
            {
                Console.LogWarning("Background Music is not using a Switch Container. Time of day will be ignored.");
                ap.Container.Loop = -1;
                Destroy(this);
            }

            if (time == null)
            {
                Console.LogWarning("Cannot find Time Controller. Background music will ignore time of day.");
                ap.Container.Loop = -1;
                Destroy(this);
            }

            if (_tracks.Count <= 0) 
            {
                Console.LogError("No tracks for time of day are set. Background music will ignore time of day.");
                ap.Container.Loop = -1;
                Destroy(this);
            }
            _tracks.Sort();
            PlayMusic();
        }

        private void FixedUpdate()
        {
            int expectedTrack = ResolveCurrentTrack();
            if (lastTrack != expectedTrack)
            {
                lastTrack = expectedTrack;

                // Will be replaced with a change signal in the containers themselves
                ap.Stop();
                // Stopping happens to autoplay the next track
            }
        }

        void PlayMusic() 
        {
            switchContainer.Selection = ResolveCurrentTrack();
            ap.Play();
        }

        int ResolveCurrentTrack() 
        {
            float currentTime = time.GetTime();
            TrackTime current = _tracks[_tracks.Count-1];
            foreach (TrackTime tt in _tracks) 
            {
                if (tt.Time <= currentTime)
                {
                    current = tt;
                }
                else 
                {
                    break;                }
            }
            return current.Track;
        }
    }
}