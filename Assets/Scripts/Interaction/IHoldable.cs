using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interaction
{
    public interface IHoldable
    {
        public string HoldText { get; }
        public void PickUp();
        public void PutDown();
        public void Shake();
    }
}