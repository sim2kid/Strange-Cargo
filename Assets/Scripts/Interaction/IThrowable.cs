using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interaction
{
    public interface IThrowable : IHoldable
    {
        public string ThrowText { get; }
        public void Throw();
        public void HoldUpdate();
    }
}
