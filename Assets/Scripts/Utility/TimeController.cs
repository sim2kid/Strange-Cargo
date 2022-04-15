using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PersistentData.Saving;
using PersistentData.Component;

namespace Utility
{
    [ExecuteAlways]
    public class TimeController : MonoBehaviour, ISaveable
    {
        /// <summary>
        /// Seconds Elapsed
        /// </summary>
        private float _time { get => data.Time; set => data.Time = value; }

        public delegate void DayPassed();
        public static event DayPassed OnDayPass;

        [SerializeField]
        TimeData data;

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(data._guid))
            {
                data._guid = System.Guid.NewGuid().ToString();
            }
        }

        [Tooltip("What time should the game start at?")]
        [SerializeField]
        [Range(0f, 24f)]
        private float StartTime;


        [Tooltip("Returns the current time in a 0-24 hour format")]
        [SerializeField]
        [Range(0f,24f)]
        public float CurrentTime;

        private float _expectedTime;

        
        /// <summary>
        /// The time in a 0-1 format
        /// </summary>
        public float DayProgress { get => CurrentTime/24f; }
        public ISaveData saveData { get => data; set => data = (TimeData)value; }

        [Tooltip("The real life minutes to hour per Day/Night cycle")]
        public float MinutesInADay = 15f;

        private void OnEnable()
        {
            if (!Application.IsPlaying(gameObject))
                return;
            // Sets the time controller in the toolbox
            Toolbox.Instance.TimeController = this;
        }

        void Start()
        {
            SetTime(StartTime);
            CurrentTime = GetTime();
            _expectedTime = CurrentTime;
        }

        // Update is called once per frame
        void Update()
        {
            if (!Application.IsPlaying(gameObject)) 
            {
                SetTime(CurrentTime);
                return;
            }

            if (Utility.Toolbox.Instance.Pause.Paused)
                return;

            // Check if the editor changed the current time
            if (CurrentTime != _expectedTime)
            {
                // Set the time
                SetTime(CurrentTime);
            }

            // Gives seconds elapsed
            _time += Time.deltaTime;

            if (_time > MinutesInADay * 60)
            {
                _time -= MinutesInADay * 60;
                OnDayPass?.Invoke();
            }

            CurrentTime = GetTime();
            _expectedTime = CurrentTime;
        }

        /// <summary>
        /// Returns the time between 0-24 representing the hours of a day
        /// </summary>
        /// <returns></returns>
        public float GetTime()
        {
            return (_time / (MinutesInADay * 60)) * 24;
        }

        /// <summary>
        /// Sets the time based on the hour of the day.
        /// </summary>
        /// <param name="hour"></param>
        public void SetTime(float hour) 
        {
            _time = MinutesInADay * (hour/24f) * 60;
        }

        public override string ToString() 
        {
            int hour = Mathf.FloorToInt(CurrentTime);
            int minute = Mathf.FloorToInt((CurrentTime * 60) % 60);
            bool morning = hour < 12;
            if (!morning)
                hour -= 12;
            if (hour == 0) 
            {
                hour = 12;
            }
            return $"{hour}:{minute.ToString("00")} {(morning?"AM":"PM")}";
        }

        public void PreSerialization()
        {
            return;
        }
        public void PreDeserialization()
        {
            return;
        }
        public void PostDeserialization()
        {
            CurrentTime = GetTime();
            _expectedTime = CurrentTime;
        }
    }
}