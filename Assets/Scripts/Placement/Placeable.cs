using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interaction;
using UnityEngine.Events;

namespace Placement
{
    public class Placeable : Pickupable, IUseable
    {
        public UnityEvent OnUse;

        [SerializeField]
        string useString;
        public string UseText => useString;

        public void HoldUpdate()
        {
            
        }

        public void Use()
        {
            
        }
    }
}