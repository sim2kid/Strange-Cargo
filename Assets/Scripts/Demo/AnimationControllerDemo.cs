using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class AnimationControllerDemo : MonoBehaviour
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
        SetAnimatorIsMovingBool();
    }

    void SetAnimatorIsMovingBool()
    {
        if(navMeshAgent.velocity == Vector3.zero)
        {
            Debug.Log("NavMeshAgent is stopped");
            animator.SetBool("IsMoving", false);
        }
        else
        {
            Debug.Log("NavMeshAgent is in motion");
            animator.SetBool("IsMoving", true);
        }
    }
}
