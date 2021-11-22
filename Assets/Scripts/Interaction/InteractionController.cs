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
        public GameObject Eyes;
        [Tooltip("The distance from the Eyes that the player can interact with")]
        [SerializeField]
        float InteractionDistance = 3f;

        private PlayerInput playerInput;
        private InputAction interact;
        private Utility.ToolTip tt; 

        List<IInteractable> UpClickQueue;
        public IInteractable Previous { get; private set; }

        bool lastButton;

        private void Start()
        {
            tt = Utility.Toolbox.Instance.ToolTip;
            lastButton = false;
            playerInput = GetComponent<PlayerInput>();
            interact = playerInput.actions["Use"];
            UpClickQueue = new List<IInteractable>();
        }

        void Update()
        {
            if(tt == null)
                tt = Utility.Toolbox.Instance.ToolTip;

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

            bool buttonDown = interact.ReadValue<float>() == 1;

            if (hitObj != null)
            {
                IInteractable closest = hitObj.GetComponent<IInteractable>();

                if (Previous != closest)
                {
                    if (Previous != null)
                    {
                        if (Previous.ClickState == ClickState.Down || Previous.ClickState == ClickState.Hold)
                            UpClickQueue.Add(Previous);
                        Previous.Exit();
                        tt.HoverText = string.Empty;
                    }
                    Previous = closest;
                    closest.Enter();
                    tt.HoverText = closest.InteractText;
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
                if (Previous != null)
                {
                    if (Previous.ClickState == ClickState.Down || Previous.ClickState == ClickState.Hold)
                        UpClickQueue.Add(Previous);
                    Previous.Exit();
                    tt.HoverText = string.Empty;
                    Previous = null;
                }
            }

            if (buttonDown == false && lastButton == true)
            {
                while (UpClickQueue.Count > 0) 
                {
                    UpClickQueue[0].Up();
                    UpClickQueue.RemoveAt(0);
                }
            }
            lastButton = buttonDown;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            if (Previous != null)
            {
                if (Previous.ClickState != ClickState.Exit && Previous.ClickState != ClickState.None)
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