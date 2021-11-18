using Genetics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using Creature.Stats;
using Creature.Task;
using Creature.Brain;
using UnityEngine.Events;

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

        public Animator Animator { get; private set; }

        Queue<ITask> tasks;
        private int maxTasks = 10;
        private float maxTimeOnTask = 15f;

        private float timeSpentOnLastTask;

        private IProgress textureController;
        
        private UnityEvent UpdateLoop;

        [SerializeField, HideInInspector]
        private BasicBrain brain;

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

        public void RequestMoreTaskTime(float requestedTime) 
        {
            timeSpentOnLastTask -= requestedTime;
        }

        public void SetUp(DNA dna, Animator animator) 
        {
            this.dna = dna;
            this.Animator = animator;
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
            UpdateLoop = new UnityEvent();
            timeSpentOnLastTask = 0;

            Debug.LogWarning("The Creature Controller has hardwritten values!!");
        }

        private void Update()
        {
            LoadingProgress = Report();
            DecayNeeds();
            UpdateLoop.Invoke();
            RunTasks();
        }

        private void RunTasks() 
        {
            timeSpentOnLastTask += Time.deltaTime;
            if (tasks.Count > 0)
            {
                if (!tasks.Peek().IsStarted)
                {
                    Debug.Log($"New Task: {tasks.Peek().GetType()}");
                    tasks.Peek().RunTask(this, UpdateLoop);
                    timeSpentOnLastTask = 0;
                }
                else if (tasks.Peek().IsDone || timeSpentOnLastTask > maxTimeOnTask)
                {
                    Debug.Log($"End of Task: {tasks.Peek().GetType()}");
                    tasks.Peek().EndTask(UpdateLoop);
                    tasks.Dequeue();
                }
            }
        }

        private void StopNormalTask() 
        {
            if(tasks.Count > 0)
                tasks.Peek().EndTask(UpdateLoop);
        }

        public void VoidTask() 
        {
            ITask task;
            if (hotTasks.Count > 0)
                task = hotTasks.Peek();
            else 
                task = tasks.Peek();


            Debug.Log($"End of Task: {task.GetType()}");
            task.EndTask(UpdateLoop);
            if (hotTasks.Count > 0)
                hotTasks.Dequeue();
            else
                tasks.Dequeue();
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
