using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Saving
{
    [System.Serializable]
    public class SettingsData
    {
        public float MusicVolume = 1f;
        public float PlayerSpeed = 5f;
        public float MouseSensitivity = 20f;
    }
}