using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Slider))]
    [ExecuteAlways]
    public class NeedsGradient : MonoBehaviour
    {
        public Gradient color;
        private Slider slider;
        public Image image;

        private float value => (slider.value - slider.minValue) / (slider.maxValue - slider.minValue);

        // Start is called before the first frame update
        void Start()
        {
            slider = GetComponent<Slider>();
        }

        // Update is called once per frame
        void Update()
        {
            if (image != null)
                image.color = color.Evaluate(value);
        }
    }
}