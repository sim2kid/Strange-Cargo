using Newtonsoft.Json;
using PersistentData.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Component
{
    public class Price : MonoBehaviour, ISaveable
    {
        [SerializeField]
        FloatListData _floats = new FloatListData();

        public static Price Instance{ get; private set; }

        [SerializeField]
        private float _PriceREADONLY;

        public ISaveData saveData { get => _floats; set { _floats = (FloatListData)value; } }

        public float Value
        {
            get
            {
                if (_floats.FloatData.TryGetValue("Price", out float value))
                {
                    return value;
                }
                _floats.FloatData.Add("Price", 0);
                return 0;
            }
        }

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


        public void PostDeserialization()
        {

        }

        public void PreDeserialization()
        {

        }

        public void PreSerialization()
        {

        }
    }
}
