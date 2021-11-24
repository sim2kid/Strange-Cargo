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
        [SerializeField]
        public string Guid;
        [SerializeField]
        public string frontFeetSound;
        [SerializeField]
        public string backFeetSound;

        public NavMeshMovement Move { get; private set; }
        public int TaskCount => tasks.Count;
        public ITask TopTask => tasks.Peek();

        public Animator Animator { get; private set; }

        Queue<ITask> tasks;
        Queue<ITask> hotTasks;

        private int maxTasks = 10;
        private float maxTimeOnTask = 15f;

        [SerializeField]
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
        private Needs needsDecayRate = new Needs(
            -0.5f, // Appetite
            0,//-0.1f, // Bladder
            0,//-0.1f, // Social
            0,//-0.1f, // Energy
            0,//-0.1f // Hygiene
            0 // Happiness
        );

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
            needs += needChanges.NeedChange;
        }

        public bool AddTask(ITask task) 
        {
            if (Toolbox.Instance.Pause.Paused)
                return false;
            if (tasks.Count < maxTasks) 
            {
                tasks.Enqueue(task);
                return true;
            }
            return false;
        }

        public bool AddHotTask(ITask task)
        {
            if (Toolbox.Instance.Pause.Paused)
                return false;
            if (hotTasks.Count < maxTasks)
            {
                StopNormalTask();
                hotTasks.Enqueue(task);
                return true;
            }
            return false;
        }

        public void SetUp(DNA dna, Animator animator, string guid, string frontFeetSound, string backFeetSound) 
        {
            this.dna = dna;
            this.Animator = animator;
            this.Guid = guid;
            this.frontFeetSound = frontFeetSound;
            this.backFeetSound = backFeetSound;
        }
        
        public void RequestMoreTaskTime(float requestedTime) 
        {
            timeSpentOnLastTask -= Mathf.Clamp(requestedTime, 0, float.MaxValue);
        }

        private void Awake()
        {
            Console.HideInDebugConsole();
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

            Toolbox.Instance.Pause.OnPause.AddListener(OnPause);
            Toolbox.Instance.Pause.OnUnPause.AddListener(OnUnPause);

            Console.LogWarning("The Creature Controller has hardwritten values!!");
            Console.Log($"Creature [{Guid}] has been loaded into the scene at {transform.position.ToString()}.");
        }

        private void OnPause() 
        {
            Animator.enabled = false;
        }

        private void OnUnPause() 
        {
            Animator.enabled = true;
        }

        private void Update()
        {
            if (Toolbox.Instance.Pause.Paused)
                return;
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
                    Console.LogDebug($"Creature [{Guid}]: New Task: {task.GetType()}");
                    task.RunTask(this, UpdateLoop);
                    timeSpentOnLastTask = 0;
                }
                else if (task.IsDone || timeSpentOnLastTask > maxTimeOnTask)
                {
                    if (timeSpentOnLastTask > maxTimeOnTask)
                        Console.LogDebug($"Creature [{Guid}]: Task Timedout: {task.GetType()}");
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


            Console.LogDebug($"Creature [{Guid}]: End of Task: {task.GetType()} TimeLeft: {maxTimeOnTask - timeSpentOnLastTask}");
            task.EndTask(UpdateLoop);
            if (hotTasks.Count > 0)
                hotTasks.Dequeue();
            else
                tasks.Dequeue();
        }

        private void DecayNeeds() 
        {
            Needs needDecay = needsDecayRate.Clone();
            for (int i = 0; i < needDecay.Count; i++)
                needDecay[i] *= Time.deltaTime;
            needs += needDecay;
        }

        public void AnimationTrigger(string _triggerName)
        {
            Animator.SetTrigger(_triggerName);
        }
    }
}
