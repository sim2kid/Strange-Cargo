using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interaction;
using UnityEngine.Events;
using PersistentData.Saving;
using PersistentData.Component;

namespace Environment {

    public class Door : Interactable, ISaveable
    {
        public IsActiveData data;

        [SerializeField]
        List<Animator> animators;

        [SerializeField]
        bool isOpen { get => data.IsActive; set => data.IsActive = value; }

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(data._guid))
            {
                data._guid = System.Guid.NewGuid().ToString();
            }
        }

        [SerializeField]
        public UnityEvent OnClose;
        [SerializeField]
        public UnityEvent OnOpen;

        public ISaveData saveData { get => data; set => data = (IsActiveData)value; }

        protected override void Start()
        {
            Close();
            if (animators.Count == 0)
            {
                Console.LogError($"Must have an animator on a door. This effected {gameObject.name}.");
                Destroy(this);
            }
            foreach (Animator a in animators)
                a.applyRootMotion = true;
            base.Start();
        }

        public override void Down()
        {
            Toggle();
            base.Down();
        }

        public void Open() 
        {
            isOpen = true;
            OnOpen.Invoke();
        }

        public void Close() 
        {
            isOpen = false;
            OnClose.Invoke();
        }

        public void Toggle() 
        {
            if (!isOpen)
                Open();
            else
                Close();
        }

        private void Update()
        {
            foreach(Animator animator in animators)
                animator.SetBool("IsOpen", isOpen);
        }

        public void PreSerialization()
        {
            return;
        }

        public void PreDeserialization()
        {
            return;
        }

        public void PostDeserialization()
        {
            return;
        }
    }
}