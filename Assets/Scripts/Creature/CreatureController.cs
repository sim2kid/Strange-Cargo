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
        public int TaskCount => tasks.Count;
        public ITask TopTask => tasks.Peek();

        public Animator Animator { get; private set; }

        Queue<ITask> tasks;
        Queue<ITask> hotTasks;

        private int maxTasks = 10;
        private float maxTimeOnTask = 15f;

        private float timeSpentOnLastTask;

        private IProgress textureController;
        
        private UnityEvent UpdateLoop;

        private float thinkTimer;
        [SerializeField]
        private float thinkRate = 2f;

        [SerializeField, HideInInspector]
        private BasicBrain brain;

        /// <summary>
        /// Decay rate per second.
        /// </summary>
        private float[] needsDecayRate = 
        {
            -0.5f, // Appetite
            0,//-0.1f, // Bladder
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

        public bool AddHotTask(ITask task)
        {
            if (hotTasks.Count < maxTasks)
            {
                StopNormalTask();
                hotTasks.Enqueue(task);
                return true;
            }
            return false;
        }

        public void SetUp(DNA dna, Animator animator) 
        {
            this.dna = dna;
            this.Animator = animator;

        }
        
        public void RequestMoreTaskTime(float requestedTime) 
        {
            timeSpentOnLastTask -= Mathf.Clamp(requestedTime, 0, float.MaxValue);
        }

        private void Awake()
        {
            tasks = new Queue<ITask>();
            hotTasks = new Queue<ITask>();
            needs = new Needs();
            brain = new BasicBrain(this);
            Utility.Toolbox.Instance.CreatureList.Add(this);
            thinkTimer = 0;
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
            thinkTimer += Time.deltaTime;
            LoadingProgress = Report();
            DecayNeeds();
            UpdateLoop.Invoke();
            RunTasks();
            if (thinkTimer > thinkRate && tasks.Count + hotTasks.Count == 0)
            {
                thinkTimer = 0;
                brain.Think();
            }
        }

        private void RunTasks() 
        {
            timeSpentOnLastTask += Time.deltaTime;
            if (tasks.Count + hotTasks.Count > 0)
            {
                ITask task;
                if (hotTasks.Count > 0)
                    task = hotTasks.Peek();
                else
                    task = tasks.Peek();

                if (!task.IsStarted)
                {
                    //Debug.Log($"New Task: {task.GetType()}");
                    task.RunTask(this, UpdateLoop);
                    timeSpentOnLastTask = 0;
                }
                else if (task.IsDone || timeSpentOnLastTask > maxTimeOnTask)
                {
                    VoidTask();
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


            //Debug.Log($"End of Task: {task.GetType()}");
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
