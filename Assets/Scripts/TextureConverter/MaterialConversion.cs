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
        /// If the conversion texture is recorded, the hash for that texture will be stored here.
        /// If null, the hash is known
        /// </summary>
        [SerializeField]
        public string TextureHash;

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
            OriginalTexture = GetMainTexture();
        }

        /// <summary>
        /// Run a conversion on this file.
        /// </summary>
        public void Convert()
        {
            if(OriginalTexture == null)
                OriginalTexture = GetMainTexture();

            Finished = false;
            // Take time to update the texture
            ModifyTexture(OriginalTexture, colors, OverlayTexture);
        }

        /// <summary>
        /// Returns the mainTexture of the material.
        /// Check if the conversion process is complete if you want a converted texture.
        /// </summary>
        /// <returns></returns>
        public Texture2D GetMainTexture() 
        {
            return (Texture2D)this.GetComponent<Renderer>().material.mainTexture;
        }

        /// <summary>
        /// Sets the main texture to <paramref name="newTexture"/>
        /// </summary>
        /// <param name="newTexture"></param>
        public void SetMainTexture(Texture2D newTexture, string newHash = null) 
        {
            this.GetComponent<Renderer>().material.mainTexture = newTexture;
            if(!string.IsNullOrEmpty(newHash))
                TextureHash = newHash;
        }

        public float Report() 
        {
            if (Finished)
                return 1;
            return progress;
        }

        /// <summary>
        /// Sets the main texture while updating the conversion events
        /// </summary>
        /// <param name="newTexture"></param>
        private void ReapplyTexture(Texture2D newTexture)
        {
            // Set the main texture to our new texture
            SetMainTexture(newTexture);
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