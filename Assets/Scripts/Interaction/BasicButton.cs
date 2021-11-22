using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Interaction {
    public class BasicButton : BasicInteractable
    {
        [SerializeField]
        Vector3 PressedOffset;
        [SerializeField]
        float ease;

        [SerializeField]
        UnityEvent OnPress;

        Vector3 UnpressedPosition;

        bool isPressed => ClickState == ClickState.Hold;

        protected override void Start()
        {
            UnpressedPosition = transform.localPosition;
            base.Start();
        }

        // Update is called once per frame
        void Update()
        {
            if (isPressed)
                transform.localPosition -= (transform.localPosition - PressedOffset) * ease * Time.deltaTime;
            else
                transform.localPosition -= (transform.localPosition - UnpressedPosition) * ease * Time.deltaTime;
        }

        public override void Down()
        {
            OnPress.Invoke();
            Console.Debug($"Button Pressed");
            base.Down();
        }
    }
}