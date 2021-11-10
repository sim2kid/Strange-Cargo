using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Creature.NavMeshMovement))]
public class NavMeshDemo : MonoBehaviour
{
    [SerializeField]
    Transform destination;

    Creature.NavMeshMovement nav;

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<Creature.NavMeshMovement>();
        //MoveAgent();
    }

    public void MoveAgent() 
    {
        nav.MoveTo(destination);
    }
}
