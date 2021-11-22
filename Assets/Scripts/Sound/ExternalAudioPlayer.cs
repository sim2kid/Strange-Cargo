using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sound
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(ExternalSound))]
    public class ExternalAudioPlayer : MonoBehaviour, IAudioPlayer
    {
        public bool IsPlaying => source.isPlaying;
        public bool IsDelayed { get; protected set; }

        private AudioSource source;
        private float _delay;

        [SerializeField]
        public ExternalSound Sound;

        public bool DelayAfter = false;

        public UnityEvent OnPlay;

        private float _volume = 1;
        private float _clipVolume = 1;
        public float Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                source.volume = _clipVolume * _volume;
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
                if (clip != null)
                {
                    source.PlayOneShot(clip, _clipVolume * Volume);
                    OnPlay.Invoke();
                }
                else if (!IsDelayed)
                    StartCoroutine("PlayDelay");
        }

        public void PlayFrom(string repository)
        {
            Sound.LoadAudio(repository);
            Play();
        }

        private IEnumerator PlayDelay()
        {
            IsDelayed = true;
            if (!DelayAfter)
                yield return new WaitForSeconds(_delay);

            AudioClip clip = Sound.Clip;
            if (IsDelayed)
                if (clip != null)
                {
                    source.PlayOneShot(clip, Sound.Volume.Read());
                    OnPlay.Invoke();
                }

            if (DelayAfter)
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

        private void Start()
        {
            source = GetComponent<AudioSource>();
            Sound = GetComponent<ExternalSound>();
            Sound.LoadAudio();
        }

        private void FixedUpdate()
        {
            if (Sound.Loop && !IsPlaying && !IsDelayed)
                Play();
        }
    }
}