using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Utility
{
    public class Pause : MonoBehaviour
    {
        [SerializeField]
        bool _pause;

        [SerializeField]
        public UnityEvent OnPause;
        [SerializeField]
        public UnityEvent OnUnPause;

        public bool Paused => _pause;

        private void Awake()
        {
            _pause = false;
            Utility.Toolbox.Instance.Pause = this;
        }

#if UNITY_EDITOR
        bool lastPause = false;
        public void Update()
        {
            if (_pause != lastPause)
            {
                lastPause = _pause;
                _pause = !_pause;
                SetPause(!_pause);
            }
        }
#endif

        private void OnApplicationPause(bool pause)
        {
            if (pause && !Paused) 
            {
                SetPause(pause);
            }

        }

        public void SetPause(bool value) 
        {
            if (value == true && Paused != value) 
            {
                OnPause.Invoke();
                Console.Log($"The game has been paused.");
            }
            if (value == false && Paused != value)
            {
                OnUnPause.Invoke();
                Console.Log($"The game has been unpaused.");
            }
            _pause = value;
        }
    }
}