using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Interaction;
using UnityEngine.Events;
using Creature.Brain;
using DataType;
using Creature.Stats;
using Creature.Task;
using Environment;
using PersistentData.Component;

namespace Interaction
{
    [RequireComponent(typeof(Rigidbody))]
    public class Throwable : Pickupable, IThrowable, IUtility, IObject
    {
        [SerializeField]
        private float throwHeight = 5;

        [SerializeField]
        private float throwForce = 5;

        [SerializeField]
        private float fetchableCooldown = 20f;
        private float timer = 0;



        public UnityEvent OnThrow;
        public string ThrowText => throwString;

        public ValueRange BaseUtility => 10;

        public Needs NeedsFromFetch = new Needs(0,0,25,0,0,0);
        public Needs StatsEffect => NeedsFromFetch;

        public ITask RelatedTask => new Creature.Task.Fetch(this, player.gameObject);

        public IObject RelatedObject => this;

        public string Name { get => gameObject.name; set { gameObject.name = value; } }

        public string Guid { get => GetComponent<PrefabSaveable>().Data.GUID; }

        private string throwString;
        private new Rigidbody rigidbody;

        protected override void Start()
        {
            throwString = string.Empty;
            player = Utility.Toolbox.Instance.Player;
            rigidbody = this.GetComponent<Rigidbody>();
            base.Start();
            this.OnPickup.AddListener(Timeout);
        }

        private void Update()
        {
            if (timer > 0 && !Utility.Toolbox.Instance.Pause.Paused)
            {
                timer -= Time.deltaTime;
                if (timer <= 0) 
                {
                    Timeout();
                }
            }
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
            if (!Utility.Toolbox.Instance.AvalibleTasks.Contains(this))
            {
                Utility.Toolbox.Instance.AvalibleTasks.Add(this);
                timer = fetchableCooldown;
            }
        }

        private void Timeout() 
        {
            if (Utility.Toolbox.Instance.AvalibleTasks.Contains(this))
                Utility.Toolbox.Instance.AvalibleTasks.Remove(this);
        }

        public bool Equals(IObject other)
        {
            throw new System.NotImplementedException();
        }
    }
}
