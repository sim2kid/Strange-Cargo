using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Genetics;

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

        private Toolbox()
        {
            GenePool = new GeneticRepository();
        }
    }
}