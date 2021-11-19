using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Interaction
{
    [RequireComponent(typeof(Rigidbody))]
    public class Pickup : Interactable
    {
        private Rigidbody rigidbody;
        private GameObject hand;

        //get references to the rigidbody component and the player's hand object
        new private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            hand = GameObject.Find("Hand");
        }

        //if player clicks on this object, make this object a child of the player's hand and teleport it to the hand
        public override void Down()
        {
            base.Down();
            this.gameObject.transform.position = hand.transform.position;
            rigidbody.useGravity = false;
            this.gameObject.transform.parent = hand.transform;
            rigidbody.freezeRotation = true;
        }

        //keep trying to move the object towards the hand as long as the player continues holding it
        public override void Hold()
        {
            base.Hold();
            rigidbody.MovePosition(hand.transform.position);
        }

        //drop this object if the player lets go of mouse
        public override void Up()
        {
            base.Up();
            DropObject();
        }

        //if the player loses sight of this object, drop it
        public override void Exit()
        {
            base.Exit();
            if(hand.transform.childCount > 0)
            {
                DropObject();
            }
        }
        
        //de-child this object and drop it
        private void DropObject()
        {
            hand.transform.DetachChildren();
            rigidbody.useGravity = true;
            rigidbody.freezeRotation = false;
        }
    }
}
