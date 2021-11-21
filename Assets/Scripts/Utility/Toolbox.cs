using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Genetics;
using Sound;
using Creature;
using Creature.Brain;

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
        /// </summary>
        public Player.PlayerController Player { get; set; }

        public ToolTip ToolTip { get; set; }

        private Toolbox()
        {
            GenePool = new GeneticRepository();
            Genetics.Importer.Import(GenePool);
            SoundPool = new SoundRepository();
            CreatureList = new List<CreatureController>();

            AvalibleTasks = new List<IUtility>();
            AvalibleTasks.Add(new BasicUtility(new DataType.Range(0f, 5f), new Creature.Task.Wait(new DataType.Range(1f, 8f))));
            AvalibleTasks.Add(new BasicUtility(new DataType.Range(-3f, 10f), new Creature.Task.Wander(3f)));
        }
    }
}