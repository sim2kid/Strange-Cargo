using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interaction;
using Player;
using Creature.Stats;
using UnityEngine.Events;
using Creature;

namespace Environment
{
    public class Brush : Pickupable, IUseable
    {
        public UnityEvent OnUse;
        public string UseText => useString;

        private PlayerController Player;

        private string useString;

        public float brushRange = 3;

        public float hygieneBoostAmount = 10;

        public void HoldUpdate()
        {
            if (CanSeeCreature())
            {
                useString = "{use} to brush";
            }
            else
            {
                useString = string.Empty;
            }
        }

        private bool CanSeeCreature()
        {
            if(NearestCreature() != null)
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
            Ray ray = new Ray(Player.Eyes.transform.position, Player.Eyes.transform.forward);
            RaycastHit[] hits = Physics.RaycastAll(ray, brushRange);
            List<RaycastHit> creatures = new List<RaycastHit>();

            foreach (RaycastHit hit in hits)
            {
                CreatureController creature = hit.transform.gameObject.GetComponent<CreatureController>();

                if (creature != null)
                {
                    creatures.Add(hit);
                }
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

        public void Use()
        {
            if(CanSeeCreature())
            {
                OnUse.Invoke();
                NearestCreature().needs.Hygiene += hygieneBoostAmount;
            }
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            useString = string.Empty;
            Player = Utility.Toolbox.Instance.Player;
            base.Start();
        }
    }
}
