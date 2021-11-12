using Genetics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using Creature.Stats;
using Creature.Task;


namespace Creature
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextureConverter.TextureController))]
    [RequireComponent(typeof(NavMeshMovement))]
    public class CreatureController : MonoBehaviour, IProgress
    {
        [SerializeField]
        public DNA dna;
        [SerializeField]
        public Needs needs;

        public NavMeshMovement Move { get; private set; }

        Queue<ITask> tasks;
        private int maxTasks = 10;

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
        public void ProcessINeed(INeedChange needChanges)
        {
            needs.AddNeeds(needChanges.NeedChange);
        }

        public bool AddTask(ITask task) 
        {
            if (tasks.Count < maxTasks) 
            {
                tasks.Enqueue(task);
                return true;
            }
            return false;
        }



        private void OnEnable()
        {
            tasks = new Queue<ITask>();
            needs = new Needs();
            Utility.Toolbox.Instance.CreatureList.Add(this);
        }

        private void Start()
        {
            textureController = GetComponent<TextureConverter.TextureController>();
            Move = GetComponent<NavMeshMovement>();

            Debug.LogWarning("The Creature Controller has hardwritten values!!");
        }

        private void Update()
        {
            LoadingProgress = Report();

            DecayNeeds();

            if (tasks.Count > 0) 
            {
                if (!tasks.Peek().IsStarted)
                {
                    tasks.Peek().RunTask(this);
                }
                else if (tasks.Peek().IsDone) 
                {
                    tasks.Dequeue();
                }
            }
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
