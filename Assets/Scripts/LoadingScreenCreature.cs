using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LoadingScreenCreature : MonoBehaviour
{
    [SerializeField]
    Animator anime;
    [SerializeField]
    NavMeshAgent move;
    [SerializeField]
    ScreenLoading loadingScreen;

    [SerializeField]
    float speedCoeff;

    [SerializeField]
    GameObject[] Path;
    [SerializeField]
    int index;

    Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
        index = 0;
    }

    void Update()
    {
        float speed = Vector3.Distance(transform.position, lastPosition);
        if(move.enabled == false)
            move.enabled = true;
        anime.SetFloat("Speed", speed * speedCoeff);
        if (Path.Length > 0) 
        {
            if (index == 0) 
            {
                move.Warp(Path[index].transform.position);
                move.SetDestination(Path[index].transform.position);
            }

            if (move.remainingDistance <= 0.2f) 
            {
                index++;
                if(index < Path.Length)
                    move.SetDestination(Path[index].transform.position);
                
            }

            if (index >= Path.Length)
            {
                index = 0;
            }
        }

        lastPosition = transform.position;
    }
}
