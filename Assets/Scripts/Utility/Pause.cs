using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class Pause : MonoBehaviour
    {
        [SerializeField]
        bool pause;

        public bool Paused => pause;

        private void Awake()
        {
            pause = false;
            Utility.Toolbox.Instance.Pause = this;
        }

        public void SetPause(bool value) 
        {
            pause = value;
        }
    }
}