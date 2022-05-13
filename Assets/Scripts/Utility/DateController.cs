using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PersistentData.Saving;
using PersistentData.Component;

namespace Utility
{
    [ExecuteAlways]
    public class DateController : MonoBehaviour, ISaveable
    {
        TimeController timeController;

        private Vector2 _date { get => CurrentDate; set => CurrentDate = value; }

        [SerializeField]
        DateData data;

        private void OnValidate()
        {
            if(string.IsNullOrWhiteSpace(data._guid))
            {
                data._guid = System.Guid.NewGuid().ToString();
            }
        }

        [Tooltip("What date should the game start at?")]
        [SerializeField]
        private Vector2 StartDate;

        [Tooltip("Returns the current date as a Vector2 in (Month, Day) format.")]
        [SerializeField]
        private Vector2 CurrentDate;

        private Vector2 _expectedDate;

        public ISaveData saveData { get => data; set => data = (DateData)value; }

        [Tooltip("The amount of days in a month.")]
        public float DaysInAMonth = 30f;

        [Tooltip("The amount of months in a year.")]
        public float MonthsInAYear = 4f;

        private void OnEnable()
        {
            if(!Application.IsPlaying(gameObject))
            {
                return;
            }
            //Sets the date controller in the toolbox
            Toolbox.Instance.DateController = this;
            timeController = Toolbox.Instance.TimeController;
            TimeController.OnDayPass += IncrementDate;

        }

        private void OnDisable()
        {
            TimeController.OnDayPass -= IncrementDate;
        }

        // Start is called before the first frame update
        void Start()
        {
            SetDate(StartDate);
            CurrentDate = GetDate();
            _expectedDate = CurrentDate;
        }

        // Update is called once per frame
        void Update()
        {
            if (!Application.IsPlaying(gameObject))
            {
                SetDate(CurrentDate);
                return;
            }

            if (Toolbox.Instance.Pause.Paused)
                return;

            // Check if the editor changed the current date
            if (CurrentDate != _expectedDate)
            {
                // Set the date
                SetDate(CurrentDate);
            }

            CurrentDate = GetDate();
            _expectedDate = CurrentDate;
        }

        /// <summary>
        /// Adds 1 to the day and moves everything else forward
        /// </summary>
        public void IncrementDate()
        {
            //Increment days by 1
            _date += new Vector2(0f, 1f);
            //Increment months by 1 if days exceed the allotted monthly amount
            if (_date.y > DaysInAMonth)
            {
                _date = new Vector2(_date.x, 1f);
                _date += new Vector2(1f, 0f);
            }
            //Reset date if months exceed the allotted yearly amount
            if(_date.x > MonthsInAYear)
            {
                _date = new Vector2(1f, 1f);
            }
        }
        /// <summary>
        /// Returns the current date as a Vector2 in (Month, Day) format
        /// </summary>
        /// <returns></returns>
        public Vector2 GetDate()
        {
            return (_date);
        }

        /// <summary>
        /// Sets the date.
        /// </summary>
        /// <param name="_date"></param>
        public void SetDate(Vector2 date)
        {
            _date.Set(date.x, date.y);
        }

        public override string ToString()
        {
            int day = (int)CurrentDate.y;
            int month = (int)CurrentDate.x;
            return $"{day.ToString("00")}/{month.ToString("00")}";
        }

        public void PostDeserialization()
        {
            _date = new Vector2(data.Day, data.Month);
        }

        public void PreDeserialization()
        {
            return;
        }

        public void PreSerialization()
        {
            data.Day = _date.x;
            data.Month = _date.y;
        }
    }
}
