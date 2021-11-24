using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CreatureAnimationControllerDemo : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        SetAnimatorVelocity();
    }

    void SetAnimatorVelocity()
    {
        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
    }
}
