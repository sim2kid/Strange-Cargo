using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class CreatureAnimationControllerDemo : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        SetAnimatorVelocity();
    }

    void SetAnimatorVelocity()
    {
        animator.SetFloat("Velocity", navMeshAgent.velocity.magnitude);
    }
}
