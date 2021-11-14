using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DemoCreature : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject creature = Genetics.CreatureGeneration.CreateCreature();
        creature.GetComponent<NavMeshAgent>().Warp(transform.position);
    }
}
