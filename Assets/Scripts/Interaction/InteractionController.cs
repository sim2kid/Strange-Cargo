using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interaction
{
    [RequireComponent(typeof(PlayerInput))]
    [DisallowMultipleComponent]
    public class InteractionController : MonoBehaviour
    {
        [Tooltip("The object to cast a Raycast from")]
        [SerializeField]
        GameObject Eyes;
        [Tooltip("The distance from the Eyes that the player can interact with")]
        [SerializeField]
        float InteractionDistance = 3f;

        private PlayerInput playerInput;
        private InputAction interact;

        IInteractable previous;

        private void Start()
        {
            playerInput = GetComponent<PlayerInput>();
            interact = playerInput.actions["Interact"];
        }

        void Update()
        {
            // Sets the direction of a ray
            Ray ray = new Ray(Eyes.transform.position, Eyes.transform.forward);


            // The results of all the hits
            RaycastHit[] hits = Physics.RaycastAll(ray, InteractionDistance);
            List<GameObject> objects = new List<GameObject>();

            foreach (RaycastHit hit in hits) 
            {
                IInteractable interact = hit.transform.gameObject.GetComponent<IInteractable>();
                if(interact != null)
                    objects.Add(hit.transform.gameObject);
            }

            GameObject hitObj = null;
            if (hits.Length > 0)
                hitObj = GetClosest(objects);

            if (hitObj != null)
            {
                IInteractable closest = hitObj.GetComponent<IInteractable>();

                bool buttonDown = interact.ReadValue<float>() == 1;

                if (previous != closest)
                {
                    if (previous != null)
                        previous.Exit();
                    previous = closest;
                    closest.Enter();
                }
                else
                {
                    // If not consistant button press
                    switch (closest.ClickState)
                    {
                        // Mouse Up Prevoius States
                        case ClickState.Hover:
                        case ClickState.Enter:
                        case ClickState.Up:
                            if (buttonDown)
                                closest.Down();
                            else
                                closest.Hover();
                            break;
                        // Mouse Down Prevoius States
                        case ClickState.Down:
                        case ClickState.Hold:
                            if (buttonDown)
                                closest.Hold();
                            else
                                closest.Up();
                            break;
                    }
                }
            }
            else 
            {
                if (previous != null)
                {
                    previous.Exit();
                    previous = null;
                }
            }
            
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            if (previous != null)
            {
                if (previous.ClickState != ClickState.Exit && previous.ClickState != ClickState.None)
                {
                    Gizmos.color = Color.red;
                }
            }

            Gizmos.DrawLine(Eyes.transform.position, Eyes.transform.position + (Eyes.transform.forward * InteractionDistance));
        }

        public GameObject GetClosest(List<GameObject> objs) 
        {
            Vector3 me = Eyes.transform.position;
            GameObject toReturn = null;
            float shortest = int.MaxValue;
            foreach (GameObject i in objs) 
            {
                Vector3 them = i.transform.position;
                float distance = Vector3.Distance(me, them);
                if (distance < shortest)
                {
                    shortest = distance;
                    toReturn = i;
                }
            }
            return toReturn;
        }
    }
}