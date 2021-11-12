using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Creature
{
    [RequireComponent(typeof(NavMeshAgent))]
    [DisallowMultipleComponent]
    public class NavMeshMovement : MonoBehaviour
    {
        [Tooltip("Internal destination for predefined movement")]
        [SerializeField]
        Transform destination;
        [Tooltip("True if the agent will move to the destination on start")]
        [SerializeField]
        bool moveOnStart = true;

        private NavMeshAgent navMeshAgent;
        // Start is called before the first frame update
        void Start()
        {
            GetNavMeshAgent();
            if (moveOnStart)
                MoveAgent();
        }

        /// <summary>
        /// Find and assign the NavMesh Agent component attached to this game object.
        /// </summary>
        void GetNavMeshAgent()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.height = 0.8f;
            navMeshAgent.speed = 1.8f;
            navMeshAgent.angularSpeed = 200;
        }

        /// <summary>
        /// Attempt to move the NavMesh agent to a specified point in 3D space.
        /// </summary>
        /// <param name="_destination">Point in 3D space the NavMesh agent will attempt to move to.</param>
        public void MoveTo(Vector3 _destination)
        {
            if (navMeshAgent == null)
                GetNavMeshAgent();
            navMeshAgent.destination = _destination;
        }
        /// <summary>
        /// Attempt to move the NavMesh agent to a specified game object in the scene.
        /// </summary>
        /// <param name="_destination">Game object in the scene the NavMesh agent will attempt to move to.</param>
        public void MoveTo(Transform _destination)
        {
            MoveTo(_destination.position);
        }

        /// <summary>
        /// Moves agent to internal transform location
        /// </summary>
        public void MoveAgent()
        {
            if (destination != null)
                MoveTo(destination);
        }
    }
}