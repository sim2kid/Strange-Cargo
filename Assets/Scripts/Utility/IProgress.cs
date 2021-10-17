using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public interface IProgress
    {
        /// <summary>
        /// Marking if a process is done or not.
        /// </summary>
        public bool Finished { get; }

        /// <summary>
        /// Grabs the current progress between [0,1]
        /// </summary>
        /// <returns></returns>
        public float Report();
    }
}