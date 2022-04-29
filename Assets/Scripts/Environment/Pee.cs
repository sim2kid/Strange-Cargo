using Creature;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    public class Pee : MonoBehaviour
    {
        bool disapear = false;
        float timeSize = 3f;
        private void OnTriggerEnter(Collider other)
        {
            CreatureController creature = other.GetComponent<CreatureController>();
            if (creature != null) 
            {
                creature.ProcessNeedChange(new Creature.Stats.Needs(0, 0, 0, 0, 
                    -30 + Random.Range(-5f,5f), 0));
            }
        }
        public void Clean() 
        {
            disapear = true;
        }
        private void Update()
        {
            if (disapear) 
            {
                timeSize -= Time.deltaTime;
                if (timeSize <= 0)
                {
                    Destroy(gameObject);
                }
                transform.localScale = Vector3.one * (timeSize/10f);
            }
        }
    }
}