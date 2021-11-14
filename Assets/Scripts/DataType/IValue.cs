using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataType
{
    public interface IValue
    {
        /// <summary>
        /// Returns the value
        /// </summary>
        /// <returns></returns>
        public float Read();
        /// <summary>
        /// Returns the max value that the type can return
        /// </summary>
        public float MaxValue { get; }
        /// <summary>
        /// Returns the min value that the type can return
        /// </summary>
        public float MinValue { get; }
    }
}