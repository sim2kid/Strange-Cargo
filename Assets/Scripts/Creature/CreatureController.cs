using Genetics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using Creature.Stats;


namespace Creature
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextureConverter.TextureController))]
    public class CreatureController : MonoBehaviour, IProgress
    {
        [SerializeField]
        public DNA dna;
        [SerializeField]
        public Needs needs;
        private IProgress textureController;
        
        /// <summary>
        /// Decay rate per second.
        /// </summary>
        private float[] needsDecayRate = 
        {
            0.5f, // Appetite
            0,//0.1f, // Bladder
            0,//-0.1f, // Social
            0,//-0.1f, // Energy
            0, // Happiness
            0,//-0.1f // Hygiene
        };

        [SerializeField]
        private float LoadingProgress;

        public bool Finished => textureController.Finished;

        public float Report()
        {
            if (Finished)
                return 1;
            return textureController.Report();
        }

        private void OnEnable()
        {
            needs = new Needs();
            Utility.Toolbox.Instance.CreatureList.Add(this);
        }

        void Start()
        {
            textureController = GetComponent<TextureConverter.TextureController>();

            Debug.LogWarning("The Creature Controller has hardwritten values!!");
        }

        private void Update()
        {
            LoadingProgress = Report();

            DecayNeeds();
        }

        private void DecayNeeds() 
        {
            float[] needDecay = (float[])needsDecayRate.Clone();
            for (int i = 0; i < needDecay.Length; i++)
                needDecay[i] *= Time.deltaTime;
            if (needs != null)
                needs.AddNeeds(needDecay);
        }
    }
}
