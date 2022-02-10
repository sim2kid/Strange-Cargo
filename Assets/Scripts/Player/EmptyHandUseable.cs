using Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creature;
using UnityEngine.Events;

namespace Player
{
    public class EmptyHandUseable : Pickupable, IUseable, IHoldable
    {
        public string UseText => useString;
        private string useString;

        [SerializeField]
        private string onCreatureHover;
        [SerializeField]
        private string onFurnatureHover;

        [SerializeField]
        private float InteractionRange = 3;

        public UnityEvent OnPet;
        public UnityEvent OnFurnatureRemove;

        public Creature.Stats.Needs creatureNeedChangeOnPet;

        private Hand hand;

        protected override void Start()
        {
            hand = GetComponentInParent<Hand>();
            base.Start();
        }

        void Update()
        {
            if (hand.Holding == null)
            {
                hand.PickUp(this);
            }
        }

        public void Use()
        {
            if (CanSeeCreature() && (hand.Holding == null || hand.Holding == this))
            {
                OnPet.Invoke();
                Creature.Stats.Needs n = NearestCreature().needs;
                n += creatureNeedChangeOnPet;
                NearestCreature().needs = n;
            }
        }
        public void Mod1Use()
        {
            if (CanPickupFurnature())
            {
                // TODO: Itemize Furnature
                Placement.Itemizable item = GrabFurnature();
                item.GenerateItem();
            }
            else 
            {
                Use();
            }
        }

        public void HoldUpdate()
        {
            if (!(hand.Holding == null || (Object)hand.Holding == this))
            {
                useString = string.Empty;
                return;
            }


            if (CanSeeCreature())
            {
                useString = onCreatureHover;
            }
            else if (CanSeeFurnature()) 
            {
                useString = onFurnatureHover;
            }
            else
            {
                useString = string.Empty;
            }
        }


        private bool CanSeeFurnature() 
        {
            Placement.Itemizable item = GrabFurnature();
            if (item != null) 
            {
                return true;
            }
            return false;
        }
        private bool CanPickupFurnature() => CanSeeFurnature() && Utility.Toolbox.Instance.Player.GlobalInteraction.Mod1Active;
        private Placement.Itemizable GrabFurnature() 
        {
            return FindClosest<Placement.Itemizable>();
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
            return FindClosest<CreatureController>();
        }
        private T FindClosest<T>() 
        {
            Ray ray = new Ray(player.Eyes.transform.position, player.Eyes.transform.forward);

            RaycastHit[] hits = Physics.RaycastAll(ray, InteractionRange);
            List<RaycastHit> items = new List<RaycastHit>();

            foreach (RaycastHit hit in hits)
            {
                T item = hit.transform.gameObject.GetComponent<T>();

                if (item != null)
                    items.Add(hit);
            }

            Queue<GameObject> hitQueue = SortByClosest(items);
            T closest = default(T);
            if (hitQueue.Count > 0)
            {
                closest = hitQueue.Peek().GetComponent<T>();
            }
            if (closest != null)
            {
                return closest;
            }
            else
            {
                return default(T);
            }
        }
        private Queue<GameObject> SortByClosest(List<RaycastHit> objs)
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
        private RaycastHit GetClosest(List<RaycastHit> objs)
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
