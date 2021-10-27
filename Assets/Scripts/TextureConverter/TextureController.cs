using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utility;

namespace TextureConverter
{
    [DisallowMultipleComponent]
    public class TextureController : MonoBehaviour, IProgress
    {
        private const int MAX_COLORS = 3;
        private List<MaterialConversion> myChildren;
        private int conversionSpeed = 40000;

        [Tooltip("If true, it goes RGB -> Colors. Vise Vera on false.")]
        [SerializeField]
        public bool ForwardConvert = true;
        [Tooltip("The colors (3) to convert the Red, Green, and Blue channels to respectivly.")]
        [SerializeField]
        public Color[] colors;
        [Tooltip("Wheather or not the script will convert textures on start")]
        [SerializeField]
        private bool convert = true;
        [Tooltip("DEPRECIATED: This is the default shader to use on every material.")]
        [SerializeField]
        public Shader shader;

        /// <summary>
        /// Displays true after the conversion has finished
        /// </summary>
        public bool Finished { get; protected set; }
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
            Finished = true;
            OnFinished = new UnityEvent();
            myChildren = new List<MaterialConversion>();

            // Random Color Picker for testing. Won't be in the build
            /*
            Array.Resize(ref colors, MAX_COLORS);
            colors[0] = RandomColorPicker.RetriveRandomColor();
            colors[1] = RandomColorPicker.RetriveRandomColor();
            colors[2] = RandomColorPicker.RetriveRandomColor();
            */
        }

        private void Start()
        {
            Renderer[] Renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer render in Renderers) 
            {
                if (shader != null) 
                {
                    render.material.shader = this.shader;
                }
                MaterialConversion converter = render.GetComponent<MaterialConversion>();
                if (converter == null)
                {
                    converter = render.gameObject.AddComponent<MaterialConversion>();
                    converter.ForwardConvert = ForwardConvert;
                }
                converter.OnFinished.AddListener(CheckIfFinished);
                myChildren.Add(converter);
            }

            if(convert)
                Convert();
        }


        /// <summary>
        /// Run a conversion on the model.
        /// </summary>
        public void Convert() 
        {
            Finished = false;
            foreach (MaterialConversion converter in myChildren) 
            {
                converter.colors = colors;
                converter.CONVERSION_SPEED = CONVERSION_SPEED/myChildren.Count;

                converter.Convert();
            }
        }

        public float Report() 
        {
            if (Finished)
                return 1;
            float temp = 0;
            foreach (MaterialConversion converter in myChildren)
            {
                temp += converter.Report();
            }
            return temp / myChildren.Count;
        }


        private void CheckIfFinished()
        {
            foreach (MaterialConversion converter in myChildren)
            {
                if (!converter.Finished)
                    return;
            }
            OnFinished.Invoke();
        }
    }
}