using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound
{
    [System.Serializable]
    public struct TrackTime
    {
        [Tooltip("Time of day the track should begin")]
        public float Time;
        [Tooltip("What track in a sequence container to play")]
        public int Track;
    }
}