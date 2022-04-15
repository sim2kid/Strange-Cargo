using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Genetics;
using Sound;
using Creature;
using Creature.Brain;
using UnityEngine.Events;

namespace Utility
{
    public sealed class Toolbox
    {
        private static readonly Lazy<Toolbox> lazy =
            new Lazy<Toolbox>(() => new Toolbox());
        /// <summary>
        /// The singleton instance of the Toolbox
        /// </summary>
        public static Toolbox Instance { get { return lazy.Value; } }
        /// <summary>
        /// The genetic repository for the application
        /// </summary>
        public GeneticRepository GenePool { get; private set; }
        /// <summary>
        /// The time contoller component
        /// </summary>
        public TimeController TimeController { get; set; }
        /// <summary>
        /// The date controller component
        /// </summary>
        public DateController DateController { get; set; }
        /// <summary>
        /// The repository for all the sound in the game.
        /// </summary>
        public SoundRepository SoundPool {  get; private set; }
        /// <summary>
        /// A list of all the creatures in the game.
        /// </summary>
        public List<CreatureController> CreatureList { get; set; }
        /// <summary>
        /// An active list of all potential tasks in the game
        /// </summary>
        public List<IUtility> AvalibleTasks { get; set; }
        /// <summary>
        /// The Player's main interface
        /// The gameobject must be present in the scene!
        /// </summary>
        public Player.PlayerController Player { get; set; }
        /// <summary>
        /// The UI for tooltips. The gameobject must be present in the scene!
        /// </summary>
        public ToolTip ToolTip { get; set; }
        /// <summary>
        /// The pausing interface. The gameobject must be present in the scene!
        /// </summary>
        public Pause Pause { get; set; }
        /// <summary>
        /// This unity event will be called when the application is being closed.
        /// </summary>
        public UnityEvent OnClosing { get; private set; }
        /// <summary>
        /// All the face images in the game.
        /// </summary>
        public Creature.Face.FaceRepository FaceRepo { get; private set; }

        private Toolbox()
        {
            GenePool = new GeneticRepository();
            Genetics.DatabaseImport.Import(GenePool);
            SoundPool = new SoundRepository();
            CreatureList = new List<CreatureController>();
            OnClosing = new UnityEvent();
            FaceRepo = new Creature.Face.FaceRepository();

            AvalibleTasks = new List<IUtility>();
            AvalibleTasks.Add(new BasicUtility(new DataType.Range(0f, 5f), new Creature.Task.Wait(new DataType.Range(1f, 8f))));
            AvalibleTasks.Add(new BasicUtility(new DataType.Range(-3f, 10f), new Creature.Task.Wander(3f)));
        }
    }
}