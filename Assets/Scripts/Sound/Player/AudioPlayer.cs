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

        private float _clipVolume;
        [SerializeField]
        private float _sourceVolume;

        public float Volume 
        {
            get => _sourceVolume;
            set => SetVolume(value);
        }

        public UnityEvent OnPlay;
        public UnityEvent OnPlayEnd;

        [SerializeReference]
        public ISound Container = new ResourceList();


        private void Awake()
        {
            SetUp();
        }
        private void Start()
        {
            _source = GetComponent<AudioSource>();
        }

        private List<SoundBite> tempBites = new List<SoundBite>();
        private void SetUp() 
        {
            tempBites = Container.Bites;
        }

        public void Play() 
        {
            if (IsPlaying) 
            {
                if(!IsDelayed && !_source.isPlaying)
                    IsPlaying = false;
            }
            if (!IsPlaying) 
            {
                _stopSignal = false;
                IsPlaying = true;
                StartCoroutine("PlayList");
            }
        }

        private void SetVolume(float volume) 
        {
            _sourceVolume = volume;
            if (_source.isPlaying)
            { 
                volume *= _clipVolume;
            }
            _source.volume = volume;
        }

        private IEnumerator PlayList() 
        {
            SetUp(); // gets the audio ready
            SoundBite[] bites = tempBites.ToArray();
            OnPlay.Invoke();
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
                if(bites[i].Clip != null)
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

        public void LegacyResourcePlay(string resourceAudioLoc) 
        {
            this.Container = new RandomContainer();
            Container.Containers.Add(new ResourceList(resourceAudioLoc));
            Play();
        }
    }
}