using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PersistentData.Component;
using PersistentData.Saving;

namespace Interaction
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent (typeof(Collider))]
    public class Pickupable : BasicInteractable, IHoldable, ISaveable
    {
        public Vector3 positionOffset;
        public Vector3 rotationOffset;

        [SerializeField]
        RigidbodyData rbData;

        [SerializeField]
        private string _textOnHold;

        public UnityEvent OnPickup;
        public UnityEvent OnPutDown;
        public UnityEvent OnShake;

        public virtual string HoldText { get => _textOnHold; }
        public ISaveData saveData 
        { get => rbData; 
            set { rbData = (RigidbodyData)value; } }

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(rbData.GUID))
            {
                rbData.GUID = System.Guid.NewGuid().ToString();
            }
        }

        protected bool holding;
        protected Player.PlayerController player;
        protected Rigidbody rb;
        protected new Collider collider;

        protected override void Start()
        {
            if(rb == null)
                rb = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();
            player = Utility.Toolbox.Instance.Player;

            Utility.Toolbox.Instance.Pause.OnPause.AddListener(OnPause);
            Utility.Toolbox.Instance.Pause.OnUnPause.AddListener(OnUnPause);

            holding = false;
            base.Start();
        }

        private void OnDisable()
        {
            Utility.Toolbox.Instance.Pause.OnPause.RemoveListener(OnPause);
            Utility.Toolbox.Instance.Pause.OnUnPause.RemoveListener(OnUnPause);
        }

        protected virtual void OnPause()
        {
            UpdateRbData();
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        private void UpdateRbData() 
        {
            rbData.IsKinematic = rb.isKinematic;
            rbData.Velocity = rb.velocity;
            rbData.Rotation = rb.angularVelocity;
            rbData.Constraints = rb.constraints;
        }

        private void ApplyRbData() 
        {
            rb.isKinematic = rbData.IsKinematic;
            rb.constraints = rbData.Constraints;
            rb.velocity = rbData.Velocity;
            rb.angularVelocity = rbData.Rotation;
        }

        protected virtual void OnUnPause()
        {
            ApplyRbData();
            rb.WakeUp();
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

        public void PreSerialization()
        {
            if(!Utility.Toolbox.Instance.Pause.Paused)
                UpdateRbData();
        }

        public void PreDeserialization()
        {
            rb = GetComponent<Rigidbody>();
            if (Utility.Toolbox.Instance.Pause.Paused)
                OnPause();
        }

        public void PostDeserialization()
        {
            if (!Utility.Toolbox.Instance.Pause.Paused)
                ApplyRbData();
        }
    }
}