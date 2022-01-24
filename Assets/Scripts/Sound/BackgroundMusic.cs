using Sound.Player;
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

        void Start()
        {
            ap = GetComponent<AudioPlayer>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}