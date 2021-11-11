using Genetics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;


namespace Creature
{
    [DisallowMultipleComponent]
    public class CreatureController : MonoBehaviour, IProgress
    {
        [SerializeField]
        public DNA dna;
        public Needs needs;
        private IProgress textureController;

        [SerializeField]
        private float LoadingProgress;

        public bool Finished => textureController.Finished;

        public float Report()
        {
            if (Finished)
                return 1;
            return textureController.Report();
        }

        void Start()
        {
            textureController = GetComponent<TextureConverter.TextureController>();
        }

        private void Update()
        {
            LoadingProgress = Report();
        }
    }
}
