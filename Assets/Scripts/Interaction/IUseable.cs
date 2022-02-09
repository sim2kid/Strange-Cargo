using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interaction
{
    public interface IUseable : IHoldable
    {
        public string UseText { get; }
        public void Use();
        public void Mod1Use();
        public void HoldUpdate();
    }
}