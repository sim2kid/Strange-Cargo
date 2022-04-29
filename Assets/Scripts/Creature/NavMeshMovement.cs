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
        private NavMeshAgent navMeshAgent;
        private Vector3 lastDestination;

        public float Speed => navMeshAgent.velocity.magnitude;
        public float Distance => navMeshAgent.remainingDistance;
        public bool pathPending => navMeshAgent.pathPending;
        public NavMeshPathStatus pathStatus => navMeshAgent.pathStatus;

        public bool CanReachDestination => navMeshAgent.hasPath;

        // Start is called before the first frame update
        void Start()
        {
            lastDestination = transform.position;
            GetNavMeshAgent();
            Utility.Toolbox.Instance.Pause.OnPause.AddListener(OnPause);
            Utility.Toolbox.Instance.Pause.OnUnPause.AddListener(OnUnPause);
        }

        private void OnPause() 
        {
            navMeshAgent.enabled = false;
        }

        private void OnUnPause() 
        {
            navMeshAgent.enabled = true;
            if (lastDestination != Vector3.zero) 
            {
                MoveTo(lastDestination);
            }
        }

        public void Stop() 
        {
            navMeshAgent.isStopped = true;
        }

        public void Go()
        {
            navMeshAgent.isStopped = false;
        }

        /// <summary>
        /// Find and assign the NavMesh Agent component attached to this game object.
        /// </summary>
        void GetNavMeshAgent()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.height = 0.8f;
            navMeshAgent.speed = 1f;
            navMeshAgent.angularSpeed = 200;
        }

        /// <summary>
        /// Stops the agent from trying to move to another location.
        /// </summary>
        public void ClearDestination() 
        {
            navMeshAgent.ResetPath();
            lastDestination = Vector3.zero;
            Stop();
        }

        /// <summary>
        /// Teleports to world position
        /// </summary>
        /// <param name="position"></param>
        public void Warp(Vector3 position) 
        {
            navMeshAgent.Warp(position);
        }


        /// <summary>
        /// Attempt to move the NavMesh agent to a specified point in 3D space.
        /// </summary>
        /// <param name="_destination">Point in 3D space the NavMesh agent will attempt to move to.</param>
        public void MoveTo(Vector3 _destination)
        {
            Go();
            if (navMeshAgent == null)
                GetNavMeshAgent();
            navMeshAgent.destination = _destination;
            lastDestination= _destination;
        }
        /// <summary>
        /// Attempt to move the NavMesh agent to a specified game object in the scene.
        /// </summary>
        /// <param name="_destination">Game object in the scene the NavMesh agent will attempt to move to.</param>
        public void MoveTo(Transform _destination)
        {
            MoveTo(_destination.position);
        }
    }
}