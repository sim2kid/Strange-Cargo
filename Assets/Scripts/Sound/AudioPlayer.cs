using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour, IAudioPlayer
    {
        public bool IsPlaying => source.isPlaying;
        public bool IsDelayed { get; protected set; }

        private AudioSource source;
        private float _delay;

        [SerializeField]
        public BasicSound Sound;

        public bool DelayAfter = false;
        public UnityEvent OnPlay;

        private float _playerVolume = 1;
        private float _clipVolume = 1;
        public float Volume
        {
            get => _playerVolume;
            set
            { 
                _playerVolume = Mathf.Clamp(value, 0, 1);
                source.volume = Mathf.Clamp(_clipVolume * _playerVolume, 0, 1);
            }
        }

        public void Pause()
        {
            IsDelayed = false;
            source.Pause();
        }

        public void Play()
        {
            source.pitch = Sound.Pitch.Read();
            _delay = Sound.Delay.Read();
            _clipVolume = Sound.Volume.Read();
            AudioClip clip = Sound.Clip;
            if (_delay <= 0)
            {
                if (clip != null)
                {
                    source.PlayOneShot(clip, Mathf.Clamp(_clipVolume * Volume, 0, 1));
                    OnPlay.Invoke();
                }
            }
            else if (!IsDelayed)
            {
                StartCoroutine("PlayDelay");
            }
        }


        public void PlayFrom(string repository) 
        {
            Sound.LoadAudio(repository);
            Play();
        }

        private IEnumerator PlayDelay() 
        {
            IsDelayed = true;
            if(!DelayAfter)
                yield return new WaitForSeconds(_delay);

            AudioClip clip = Sound.Clip;
            if (IsDelayed)
                if (clip != null)
                {
                    source.PlayOneShot(clip, Sound.Volume.Read());
                    OnPlay.Invoke();
                }

            if(DelayAfter)
                yield return new WaitForSeconds(_delay);

            IsDelayed = false;
            _delay = 0;
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

        private void Awake()
        {
            Sound.LoadAudio();
        }

        private void Start()
        {
            source = GetComponent<AudioSource>();
        }

        private void FixedUpdate()
        {
            if (Sound.Loop && !IsPlaying && !IsDelayed)
                Play();
        }
    }
}