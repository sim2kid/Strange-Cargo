using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class SingleSound : MonoBehaviour, ISound
    {
        [SerializeField]
        private float _pitch;
        [SerializeField]
        private float _volume;
        [SerializeField]
        private float _deley;
        [SerializeField]
        private bool _loop;

        public float Pitch { get => _pitch; set => _pitch = value; }
        public float Volume { get => _volume; set => _volume = value; }
        public float Delay { get => _deley; set => _deley = value; }
        public bool Loop { get => _loop; set => _loop = value; }

        public bool IsPlaying => source.isPlaying;

        public bool IsDelayed => throw new System.NotImplementedException();

        [SerializeField]
        public AudioClip AudioClip;

        private AudioSource source;


        public void Pause()
        {
            source.Pause();
        }

        public void Play()
        {
            source.pitch = Pitch;
            if (Delay <= 0)
                source.PlayOneShot(AudioClip, Volume);
            else
                PlayDelay();
        }

        private IEnumerable PlayDelay() 
        {
            yield return Delay;
            source.PlayOneShot(AudioClip, Volume);
        }

        public void Stop()
        {
            source.Stop();
        }

        public void UnPause()
        {
            source.UnPause();
        }

        // Start is called before the first frame update
        void Start()
        {
            source = GetComponent<AudioSource>();
        }
    }
}