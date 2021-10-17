using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TextureConverter
{
    [DisallowMultipleComponent]
    public class TextureController : MonoBehaviour
    {
        private const int MAX_COLORS = 3;
        private List<MaterialConversion> myChildren;
        private int conversionSpeed = 10000;

        [Tooltip("If true, it goes RGB -> Colors. Vise Vera on false.")]
        [SerializeField]
        public bool ForwardConvert = true;
        [Tooltip("The colors (3) to convert the Red, Green, and Blue channels to respectivly.")]
        [SerializeField]
        public Color[] colors;

        /// <summary>
        /// Displays true after the conversion has finished
        /// </summary>
        public bool ConversionFinished { get; protected set; }
        /// <summary>
        /// The number of pixels this model will spend on converting per frame
        /// </summary>
        public int CONVERSION_SPEED { get => conversionSpeed; set { conversionSpeed = Mathf.Max(value, 0); } }
        /// <summary>
        /// This will run when all conversions are finished
        /// </summary>
        public UnityEvent OnFinished { get; private set; }

        private void OnValidate()
        {
            if (colors != null)
            {
                if (colors.Length > MAX_COLORS)
                {
                    Debug.LogWarning($"You can only add upto {MAX_COLORS} colors for a conversion!");
                    Array.Resize(ref colors, MAX_COLORS);
                }
            }
        }

        private void OnEnable()
        {
            OnFinished = new UnityEvent();
            myChildren = new List<MaterialConversion>();
        }

        private void Start()
        {
            Component[] Renderers = GetComponentsInChildren<Renderer>();
            foreach (Component render in Renderers) 
            {
                MaterialConversion converter = render.GetComponent<MaterialConversion>();
                if (converter == null)
                {
                    converter = render.gameObject.AddComponent<MaterialConversion>();
                    converter.ForwardConvert = ForwardConvert;
                }
                converter.OnFinished.AddListener(CheckIfFinished);
                myChildren.Add(converter);
            }

            Convert();
        }

        /// <summary>
        /// Run a conversion on the model.
        /// </summary>
        public void Convert() 
        {
            ConversionFinished = false;
            foreach (MaterialConversion converter in myChildren) 
            {
                converter.colors = colors;
                converter.CONVERSION_SPEED = CONVERSION_SPEED/myChildren.Count;
                converter.Convert();
            }
        }


        private void CheckIfFinished()
        {
            foreach (MaterialConversion converter in myChildren)
            {
                if (!converter.ConversionFinished)
                    return;
            }
            OnFinished.Invoke();
        }
    }
}