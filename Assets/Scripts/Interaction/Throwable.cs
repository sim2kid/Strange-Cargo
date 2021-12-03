using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Interaction;
using UnityEngine.Events;

namespace Interaction
{
    [RequireComponent(typeof(Rigidbody))]
    public class Throwable : Pickupable, IUseable
    {
        [SerializeField]
        private float throwHeight;

        [SerializeField]
        private float throwForce;

        public UnityEvent OnUse;
        public string UseText => useString;

        private string useString;
        private new Rigidbody rigidbody;

        protected override void Start()
        {
            useString = string.Empty;
            player = Utility.Toolbox.Instance.Player;
            rigidbody = this.GetComponent<Rigidbody>();
            base.Start();
        }

        public void HoldUpdate()
        {
        }

        public void Use()
        {
            OnUse.Invoke();
            player.HandController.LetGo();
            this.Throw();
        }

        public void Throw()
        {
            Vector3 handDirection = player.Hand.transform.forward * throwForce;
            Vector3 throwDirection = new Vector3(handDirection.x, handDirection.y + throwHeight, handDirection.z);
            rigidbody.AddForce(throwDirection, ForceMode.Impulse);
        }
    }
}
