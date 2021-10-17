using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using Utility;

namespace TextureConverter
{

    [RequireComponent(typeof(Renderer))]
    [DisallowMultipleComponent]
    public class MaterialConversion : MonoBehaviour, IProgress
    {
        private const int MAX_COLORS = 3;
        private int conversionSpeed = 1000;

        [Tooltip("Any details to be overlayed on the texture.")]
        [SerializeField]
        private Texture2D OverlayTexture = null;
        [Tooltip("If true, it goes RGB -> Colors. Vise Vera on false.")]
        [SerializeField]
        public bool ForwardConvert = true;
        [Tooltip("The colors (3) to convert the Red, Green, and Blue channels to respectivly.")]
        [SerializeField]
        public Color[] colors;

        /// <summary>
        /// [0,1] range representing how far along the conversion is.
        /// </summary>
        private float progress;

        /// <summary>
        /// The unconverted texture
        /// </summary>
        public Texture2D OriginalTexture;
        /// <summary>
        /// Displays true after the conversion has finished
        /// </summary>
        public bool Finished { get; protected set; }
        /// <summary>
        /// Controls the number of pixels processed per frame per component. 
        /// If you have a lot of componenets running at the same time, lower this number. 
        /// Set to 0 for the default speed
        /// </summary>
        public int CONVERSION_SPEED { get => conversionSpeed; set { conversionSpeed = Mathf.Max(value, 0); } }
        /// <summary>
        /// This will run when the conversion finishes
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
            CONVERSION_SPEED = 0;
            OnFinished = new UnityEvent();
        }

        private void Start()
        {
            // Get the pattern texture from the material
            OriginalTexture = (Texture2D)this.GetComponent<Renderer>().material.mainTexture;
        }

        /// <summary>
        /// Run a conversion on this file.
        /// </summary>
        public void Convert()
        {
            if(OriginalTexture == null)
                OriginalTexture = (Texture2D)this.GetComponent<Renderer>().material.mainTexture;

            Finished = false;
            // Take time to update the texture
            ModifyTexture(OriginalTexture, colors, OverlayTexture);
        }

        public float Report() 
        {
            if (Finished)
                return 1;
            return progress;
        }

        private void ReapplyTexture(Texture2D newTexture)
        {
            // Set the main texture to our new texture
            this.GetComponent<Renderer>().material.mainTexture = newTexture;
            Finished = true;
            OnFinished.Invoke();
        }

        private void GetProgress(float progress) 
        {
            this.progress = progress;
        }

        private void ModifyTexture(Texture2D toModify, Color[] newColors, Texture2D textureDetails = null)
        {
            if (ForwardConvert)
                StartCoroutine(TextureConversions.ConvertTexture(ReapplyTexture, toModify, newColors, textureDetails, CONVERSION_SPEED, GetProgress));
            else
                StartCoroutine(TextureConversions.GenerateBaseTexture(ReapplyTexture, toModify, newColors, CONVERSION_SPEED, GetProgress));

        }
    }
}