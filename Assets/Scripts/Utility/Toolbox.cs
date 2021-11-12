using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Genetics;
using Sound;
using Creature;

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

        public SoundRepository SoundPool {  get; private set; }

        public List<CreatureController> CreatureList { get; set; }

        private Toolbox()
        {
            GenePool = new GeneticRepository();
            Genetics.Importer.Import(GenePool);
            SoundPool = new SoundRepository();
            CreatureList = new List<CreatureController>();
        }
    }
}