using Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creature;
using UnityEngine.Events;

namespace Player
{
    public class PettingUseable : Pickupable, IUseable, IHoldable
    {
        public string UseText => useString;

        [SerializeField]
        private string useString;

        public float pettingRange = 3;

        public UnityEvent OnUse;

        private Hand hand;

        public float socialBoostAmount = 10;

        protected override void Start()
        {
            hand = GetComponentInParent<Hand>();
            base.Start();
        }

        void Update()
        {
            if(hand.Holding == null)
            {
                hand.PickUp(this);
            }
        }

        public void HoldUpdate()
        {
            if (CanSeeCreature() && (hand.Holding == null || hand.Holding == this))
            {
                useString = "{use} to pet creature";
            }
            else
            {
                useString = string.Empty;
            }
        }

        public void Use()
        {
            if(CanSeeCreature() && (hand.Holding == null || hand.Holding == this))
            {
                OnUse.Invoke();
                NearestCreature().needs.Social += socialBoostAmount;
            }
        }


        private bool CanSeeCreature()
        {
            if (NearestCreature() != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private CreatureController NearestCreature()
        {
            Ray ray = new Ray(player.Eyes.transform.position, player.Eyes.transform.forward);

            RaycastHit[] hits = Physics.RaycastAll(ray, pettingRange);
            List<RaycastHit> creatures = new List<RaycastHit>();

            foreach (RaycastHit hit in hits)
            {
                CreatureController creature = hit.transform.gameObject.GetComponent<CreatureController>();

                if (creature != null)
                    creatures.Add(hit);
            }

            Queue<GameObject> hitQueue = SortByClosest(creatures);

            CreatureController closest = null;
            if (hitQueue.Count > 0)
            {
                closest = hitQueue.Peek().GetComponent<CreatureController>();
            }
            if (closest != null)
            {
                return closest;
            }
            else
            {
                return null;
            }
        }

        public Queue<GameObject> SortByClosest(List<RaycastHit> objs)
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            while (objs.Count > 0)
            {
                RaycastHit obj = GetClosest(objs);
                queue.Enqueue(obj.transform.gameObject);
                objs.Remove(obj);
            }
            return queue;
        }

        public RaycastHit GetClosest(List<RaycastHit> objs)
        {
            RaycastHit toReturn = objs[0];
            float shortest = int.MaxValue;
            foreach (RaycastHit i in objs)
            {
                float distance = i.distance;
                if (distance < shortest)
                {
                    shortest = distance;
                    toReturn = i;
                }
            }
            return toReturn;
        }
    }
}
