using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DemoNavMeshMovement : MonoBehaviour
{
    [Tooltip("Point in 3D space the NavMesh agent will attempt to move to.")]
    [SerializeField] Vector3 destinationVector3;
    [Tooltip("Game object in the scene the NavMesh agent will attempt to move to.")]
    [SerializeField] Transform destinationTransform;
    [Tooltip("The NavMesh Agent component attached to this game object.")]
    private NavMeshAgent navMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        GetNavMeshAgent();
        MoveToTransform(destinationTransform);
    }
    /// <summary>
    /// Find and assign the NavMesh Agent component attached to this game object.
    /// </summary>
    void GetNavMeshAgent()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    /// <summary>
    /// Attempt to move the NavMesh agent to a specified point in 3D space.
    /// </summary>
    /// <param name="_destination">Point in 3D space the NavMesh agent will attempt to move to.</param>
    void MoveToVector3(Vector3 _destination)
    {
        navMeshAgent.destination = _destination;
    }
    /// <summary>
    /// Attempt to move the NavMesh agent to a specified game object in the scene.
    /// </summary>
    /// <param name="_destination">Game object in the scene the NavMesh agent will attempt to move to.</param>
    void MoveToTransform(Transform _destination)
    {
        navMeshAgent.destination = _destination.position;
    }
}
