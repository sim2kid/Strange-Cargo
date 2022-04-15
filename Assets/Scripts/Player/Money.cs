using PersistentData.Component;
using PersistentData.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Money : MonoBehaviour, ISaveable
    {
        public static Money Instance { get; private set; }

        public float Value
        {
            get
            {
                if (_floats.FloatData.TryGetValue("Money", out float value))
                {
                    return value;
                }
                _floats.FloatData.Add("Money", 0);
                return 0;
            }
            set
            {
                if (_floats.FloatData.ContainsKey("Money"))
                {
                    _floats.FloatData["Money"] = value;
                }
                else
                {
                    _floats.FloatData.Add("Money", value);
                }
            }
        }

        [SerializeField]
        private float _MoneyREADONLY;

        [SerializeField]
        FloatListData _floats = new FloatListData();

        private void Awake()
        {
            if (!Application.isPlaying) return;
            if (Instance == null)
            {
                Instance = this;
            }
            else 
            {
                Console.LogError("There can only be one Money. Deleting Extras.");
                Destroy(this);
            }
        }

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(_floats.GUID))
            {
                _floats.GUID = System.Guid.NewGuid().ToString();
            }
        }

        public ISaveData saveData { get => _floats; set { _floats = (FloatListData)value; } }

        public void PreSerialization()
        {
        }

        public void PreDeserialization()
        {
        }

        public void PostDeserialization()
        {
        }
        private void Update()
        {
            _MoneyREADONLY = Value;
        }
    }
}