using Genetics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using Creature.Stats;
using Creature.Task;
using Creature.Brain;
using UnityEngine.Events;
using PersistentData.Saving;
using PersistentData.Component;
using Creature.Face;
using Creature.Emotions;

namespace Creature
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextureConverter.TextureController))]
    [RequireComponent(typeof(NavMeshMovement))]
    [RequireComponent(typeof(CreatureSaveable))]
    [RequireComponent(typeof(FaceController))]
    public class CreatureController : MonoBehaviour, IProgress, ISaveable
    {
        [SerializeField]
        private CreatureData data;

        public ISaveData saveData { get => data; set { data = (CreatureData)value; } }

        public DNA dna { get => data.dna; set => data.dna = value; }
        public Needs needs { get => data.needs; set => data.needs = value; }

        public string Guid { get => GetComponent<CreatureSaveable>().Data.GUID; set => GetComponent<CreatureSaveable>().Data.GUID = value; }
        public string frontFeetSound { get => data.frontFeetSound; set => data.frontFeetSound = value; }
        public string backFeetSound { get => data.backFeetSound; set => data.backFeetSound = value; }

        public FaceController Face { get; private set; }
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

        private IEmotionCheck emotionState;

        private UnityEvent UpdateLoop;

        private float thinkTimer;
        [SerializeField]
        private float thinkRate = 2f;

        [HideInInspector]
        private BasicBrain brain { get => data.brain; set => data.brain = value; }

        /// <summary>
        /// Decay rate per second.
        /// </summary>
        private Needs needsDecayRate = new Needs(
            -0.5f, // Appetite
            0,//-0.1f, // Bladder
            -0.2f, // Social
            0,//-0.1f, // Energy
            -0.1f, // Hygiene
            0 // Happiness
        );

        [SerializeField]
        private float LoadingProgress;

        public bool Finished 
        { 
            get 
            {
                if (textureController != null)
                    return textureController.Finished;
                else
                    return true;
            } 
        }

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
            data.GUID = "232fd503-5c82-405f-8e6b-f13a11e6dfae";
            Console.HideInDebugConsole();
            tasks = new Queue<ITask>();
            hotTasks = new Queue<ITask>();
            needs = new Needs();
            brain = new BasicBrain(this);
            Utility.Toolbox.Instance.CreatureList.Add(this);
            thinkTimer = 0;
            emotionState = gameObject.GetComponent<EmotionCheck>();
        }

        private void Start()
        {
            textureController = GetComponent<TextureConverter.TextureController>();
            Move = GetComponent<NavMeshMovement>();
            Face = GetComponent<FaceController>();

            Face.GrabFace = new GrabFace();
            Face.GrabFace.Hydrate(dna);

            Face.EmotionCheck = emotionState;

            UpdateLoop = new UnityEvent();
            timeSpentOnLastTask = 0;

            Toolbox.Instance.Pause.OnPause.AddListener(OnPause);
            Toolbox.Instance.Pause.OnUnPause.AddListener(OnUnPause);

            Console.LogWarning("The Creature Controller has hardwritten values!!");
            Console.Log($"Creature [{Guid}] has been loaded into the scene at {transform.position.ToString()}.");
        }

        private void OnDestroy()
        {
            Utility.Toolbox.Instance.CreatureList.Remove(this);
            VoidTask();
            Toolbox.Instance.Pause.OnPause.RemoveListener(OnPause);
            Toolbox.Instance.Pause.OnUnPause.RemoveListener(OnUnPause);
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
            data.needs = needs;
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
            ITask task = null;
            if (hotTasks.Count > 0)
                task = hotTasks.Peek();
            else 
                if(tasks.Count > 0)
                    task = tasks.Peek();
            if (task == null)
                return;

            Console.LogDebug($"Creature [{Guid}]: End of Task: {task.GetType()} TimeLeft: {maxTimeOnTask - timeSpentOnLastTask}");
            task.EndTask(UpdateLoop);
            if (hotTasks.Count > 0)
                hotTasks.Dequeue();
            else
                tasks.Dequeue();
            AnimationBool("Stop", true);
        }

        private void DecayNeeds() 
        {
            Needs needDecay = needsDecayRate.Clone();
            for (int i = 0; i < needDecay.Count; i++)
                needDecay[i] *= Time.deltaTime;
            needs += needDecay;
            needs.Clamp();
        }

        public void AnimationBool(string _boolName, bool _boolValue)
        {
            Animator.SetBool(_boolName, _boolValue);
        }
        public void AnimationTrigger(string _triggerName)
        {
            AnimationBool("Stop", false);
            Animator.SetTrigger(_triggerName);
        }

        public void PreSerialization()
        {
            brain.PreSerialization();
        }

        public void PreDeserialization()
        {
            brain.PreDeserialization();
            return;
        }

        public void PostDeserialization()
        {
            brain.PostDeserialization(this);
            if(Utility.Toolbox.Instance.Pause.Paused)
                OnPause();
        }
    }
}
