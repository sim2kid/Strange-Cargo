using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment.Tub
{
    public class Bathtub : MonoBehaviour
    {
        [SerializeField]
        Showerhead Showerhead;
        [SerializeField]
        Shampoo SoapBottle1;
        [SerializeField]
        Shampoo SoapBottle2;

        Creature.CreatureController Creature;

        public bool active;
        public float Soap;

        public void SetActive() 
        {
            active = true;

            float mindis = float.MaxValue;
            foreach (var creature in Utility.Toolbox.Instance.CreatureList) 
            {
                var dis = Vector3.Distance(this.transform.position, creature.transform.position);
                if (dis < mindis) 
                {
                    Creature = creature;
                }
            }
            if(Creature == null)
                SetInactive();

            Creature.BrainDead = true;
            Creature.Move.MoveTo(this.transform.position);
        }

        public void SetInactive() 
        {
            active = false;
            if (Creature != null) 
            {
                Creature.BrainDead = false;
            }
            Creature = null;
        }

        public void ModSoap(float amount) 
        {
            Soap += Mathf.Clamp(amount, 0, 200);
        }

        void Start()
        {

        }

        void Update()
        {

        }
    }
}