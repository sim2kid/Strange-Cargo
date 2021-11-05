using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    public class TimeController : MonoBehaviour
    {
        /// <summary>
        /// Seconds Elapsed
        /// </summary>
        private float _time;

        [Tooltip("Returns the current time in a 0-24 hour format")]
        [SerializeField]    
        public float CurrentTime;

        [SerializeField]
        public Text TimeDisplay;

        /// <summary>
        /// The time in a 0-1 format
        /// </summary>
        public float DayProgress { get => CurrentTime/24f; }

        [Tooltip("The real life minutes to hour per Day/Night cycle")]
        public float MinutesInADay = 15f;

        private void OnEnable()
        {
            // Sets the time controller in the toolbox
            Toolbox.Instance.TimeController = this;
        }

        void Start()
        {
            _time = 0;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            // Gives seconds elapsed
            _time += Time.fixedDeltaTime;

            if (_time > MinutesInADay * 60)
            {
                _time -= MinutesInADay * 60;
            }

            CurrentTime = GetTime();

            TimeDisplay.text = CurrentTime.ToString("0.00");
        }

        /// <summary>
        /// Returns the time between 0-24 representing the hours of a day
        /// </summary>
        /// <returns></returns>
        public float GetTime()
        {
            return (_time / (MinutesInADay * 60)) * 24;
        }
    }
}