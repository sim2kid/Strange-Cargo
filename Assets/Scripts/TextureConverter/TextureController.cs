using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using Utility;

namespace TextureConverter
{
    [DisallowMultipleComponent]
    public class TextureController : MonoBehaviour, IProgress
    {
        private const int MAX_COLORS = 3;
        private List<MaterialConversion> myChildren;
        private List<MaterialConversion> leaderChildren;
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
        public int CONVERSION_SPEED { get => conversionSpeed; set { conversionSpeed = Mathf.Max(value, 1); } }
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
            leaderChildren = new List<MaterialConversion>();
        }

        private void Start()
        {
            MaterialConversion[] Converters = GetComponentsInChildren<MaterialConversion>();
            foreach (MaterialConversion cons in Converters) 
            {
                cons.OnFinished.AddListener(AfterConversion);
                myChildren.Add(cons);
            }

            if (convert)
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

                if (converter.TextureHash != null)
                {
                    if (leaderChildren.Find(x => x.TextureHash == converter.TextureHash) == null)
                    {
                        leaderChildren.Add(converter);
                    }
                }
                else
                {
                    leaderChildren.Add(converter);
                }
            }
            foreach (MaterialConversion converter in leaderChildren) 
            {
                converter.CONVERSION_SPEED = CONVERSION_SPEED / leaderChildren.Count;
                converter.Convert();
            }
        }

        public float Report() 
        {
            if (Finished)
                return 1;
            float temp = 0;
            foreach (MaterialConversion converter in leaderChildren)
            {
                temp += converter.Report();
            }
            return temp / leaderChildren.Count;
        }

        private void AfterConversion()
        {
            bool allFinished = true;
            foreach (MaterialConversion converter in leaderChildren)
            {
                if (converter.Finished)
                {
                    if(converter.TextureHash != null)
                        myChildren.Where(x => x.TextureHash == converter.TextureHash).ToList().ForEach(child =>
                        {
                            child.SetMainTexture(converter.GetMainTexture());
                        });
                }
                else 
                {
                    allFinished = false;
                }
            }
            if(allFinished)
                OnFinished.Invoke();
        }
    }
}