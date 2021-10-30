using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour, IAudioPlayer
    {
        public bool IsPlaying => source.isPlaying;
        public bool IsDelayed { get; protected set; }

        private AudioSource source;

        [SerializeField]
        public ISound Sound;

        public void Pause()
        {
            IsDelayed = false;
            source.Pause();
        }

        public void Play()
        {
            source.pitch = Sound.Pitch;
            if (Sound.Delay <= 0)
                source.PlayOneShot(Sound.Clip, Sound.Volume);
            else
                PlayDelay();
        }

        private IEnumerable PlayDelay() 
        {
            IsDelayed = true;
            yield return Sound.Delay;
            if(IsDelayed)
                source.PlayOneShot(Sound.Clip, Sound.Volume);
            IsDelayed = false;
            
        }

        public void Stop()
        {
            IsDelayed = false;
            source.Stop();
        }

        public void UnPause()
        {
            source.UnPause();
        }


        private void Start()
        {
            source = GetComponent<AudioSource>();
        }

        private void Update()
        {
            
        }
    }
}