using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Interaction;
using UnityEngine.Events;

namespace Interaction
{
    [RequireComponent(typeof(Rigidbody))]
    public class Throwable : Pickupable, IThrowable
    {
        [SerializeField]
        private float throwHeight = 5;

        [SerializeField]
        private float throwForce = 5;

        public UnityEvent OnThrow;
        public string ThrowText => throwString;

        private string throwString;
        private new Rigidbody rigidbody;

        protected override void Start()
        {
            throwString = string.Empty;
            player = Utility.Toolbox.Instance.Player;
            rigidbody = this.GetComponent<Rigidbody>();
            base.Start();
        }

        public void HoldUpdate()
        {
        }

        public void Throw()
        {
            OnThrow.Invoke();
            player.HandController.LetGo();
            Vector3 handDirection = player.Hand.transform.forward * throwForce;
            Vector3 throwDirection = new Vector3(handDirection.x, handDirection.y + throwHeight, handDirection.z);
            rigidbody.AddForce(throwDirection, ForceMode.Impulse);
        }
    }
}
