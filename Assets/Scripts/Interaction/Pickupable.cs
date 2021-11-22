using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Interaction
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent (typeof(Collider))]
    public class Pickupable : BasicInteractable, IHoldable
    {
        public Vector3 positionOffset;
        public Vector3 rotationOffset;

        [SerializeField]
        private string _textOnHold;

        public UnityEvent OnPickup;
        public UnityEvent OnPutDown;
        public UnityEvent OnShake;

        public virtual string HoldText { get => _textOnHold; }

        protected bool holding;
        protected Player.PlayerController player;
        protected Rigidbody rb;
        protected new Collider collider;

        protected override void Start()
        {
            rb = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();
            player = Utility.Toolbox.Instance.Player;
            holding = false;
            base.Start();
        }

        private void Update()
        {
            if (holding) 
            {
                transform.position = player.Hand.transform.position + positionOffset;
                transform.rotation = Quaternion.Euler(player.Hand.transform.rotation.eulerAngles + rotationOffset);
            }
        }

        public virtual void PickUp() 
        {
            player.HandController.PickUp(this);
            holding = true;
            collider.enabled = false;
            rb.useGravity = false;
            OnPickup.Invoke();
        }

        public virtual void PutDown()
        {
            holding = false;
            collider.enabled = true;
            rb.useGravity = true;
            OnPutDown.Invoke();
        }

        public virtual void Shake() 
        {
            OnShake.Invoke();
        }

        public override void Up()
        {
            base.Up();
            PickUp();
        }

        public override void LateUp()
        {
            if(holding)
                Up();
        }
    }
}