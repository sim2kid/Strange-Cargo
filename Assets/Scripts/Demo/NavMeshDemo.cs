using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Creature.NavMeshMovement))]
public class NavMeshDemo : MonoBehaviour
{
    [SerializeField]
    Transform destination;

    [SerializeField]
    bool moveOnStart = true;

    Creature.NavMeshMovement nav;

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<Creature.NavMeshMovement>();
        
        if(moveOnStart)
          MoveAgent();
    }

    public void MoveAgent() 
    {
        nav.MoveTo(destination);
    }
}
