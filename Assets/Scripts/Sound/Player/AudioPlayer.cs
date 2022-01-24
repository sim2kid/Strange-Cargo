using Sound.Source.Internal;
using Sound.Structure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace Sound.Player
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {
        public bool IsPlaying { get; private set; }
        public bool IsDelayed { get; private set; }
        private bool _stopSignal;

        private AudioSource _source;

        private float _clipVolume = 1f;
        [SerializeField]
        private float _sourceVolume = 1f;

        public float Volume 
        {
            get => _sourceVolume;
            set => SetVolume(value);
        }

        public UnityEvent OnPlay;
        public UnityEvent OnPlayEnd;

        [SerializeReference]
        public ISound Container = new ResourceList();

        private ISound activeContainer;


        private void Awake()
        {
            activeContainer = Container;
        }
        private void Start()
        {
            _source = GetComponent<AudioSource>();
        }

        public void Play(bool force, bool customContainer = false) 
        {
            if (!customContainer)
                activeContainer = this.Container;
            if (IsPlaying)
            {
                if (!IsDelayed && !_source.isPlaying)
                    IsPlaying = false;
            }
            if (!IsPlaying || force)
            {
                _stopSignal = false;
                IsPlaying = true;
                StartCoroutine("PlayList");
            }
        }

        public void Play(bool force)
        {
            Play(force, false);
        }

        public void Play() 
        {
            Play(false, false);
        }

        private void SetVolume(float volume) 
        {
            _sourceVolume = volume;
            if (_source != null)
            {
                if (_source.isPlaying)
                {
                    volume *= _clipVolume;
                }
                _source.volume = volume;
            }
        }

        private IEnumerator PlayList() 
        {
            ISound container = activeContainer;
            container.Start();
            SoundBite[] bites = container.Next()?.ToArray();
            OnPlay.Invoke();
            while (bites != null)
            {
                for (int i = 0; i < bites.Length; i++)
                {
                    IsDelayed = true;
                    yield return new WaitForSeconds(bites[i].Delay.Read());
                    IsDelayed = false;
                    if (_stopSignal)
                        break;
                    float pitch = bites[i].Pitch.Read();
                    _source.pitch = pitch;
                    _clipVolume = bites[i].Volume.Read();
                    float volume = _clipVolume * Volume;
                    if (bites[i].Clip != null)
                        _source.PlayOneShot(bites[i].Clip, volume);
                    while (_source.isPlaying)
                    {
                        yield return new WaitForFixedUpdate();
                        if (_stopSignal)
                            break;
                    }
                    if (_stopSignal)
                        break;
                }
                bites = container.Next()?.ToArray();
            }
            IsPlaying = false;
            OnPlayEnd.Invoke();
        }
        public void Stop() 
        {
            _stopSignal = true;
        }
        public void Pause() 
        {
            IsDelayed = false;
            _source.Pause();
        }
        public void UnPause() 
        {
            _source.UnPause();
        }

        [System.Obsolete("This method will cause lag when loading big audio libraries for the first time. " +
            "Either use uncompressed audio or preload them with AudioPlayer#PlayOneShot(ISound).")]
        public void PlayOneShot(string resourceAudioLoc) 
        {
            activeContainer = new RandomContainer();
            activeContainer.Containers.Add(new ResourceList(resourceAudioLoc));
            Play(true, true);
        }

        public void PlayOneShot(ISound container) 
        {
            this.activeContainer = container;
            Play(true, true);
        }

        [System.Obsolete("This method will be removed in the future. Please use AudioPlayer#PlayOneShot(ISound) instead.")]
        public void LegacyResourcePlay(string resourceAudioLoc) 
        {
            PlayOneShot(resourceAudioLoc);
        }
    }
}