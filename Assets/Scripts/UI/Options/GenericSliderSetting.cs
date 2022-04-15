using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Options
{
    public abstract class GenericSliderSetting : MonoBehaviour
    {
        private UnityEngine.UI.Slider Slider;
        protected Settings settings;

        private void OnEnable()
        {
            if (Slider != null)
            {
                Slider.value = GetValue();
            }
        }

        private void Start()
        {
            settings = Settings.Instance;
            Slider = GetComponent<UnityEngine.UI.Slider>();
            Slider.value = GetValue();
            Slider.onValueChanged.AddListener(OnValueChange);
        }

        public void OnValueChange(float value) 
        {
            if (GetValue() != Slider.value)
            {
                ApplyValue(Slider.value);
            }
        }

        abstract public void ApplyValue(float value);
        abstract public float GetValue();
    }
}