using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using PersistentData;

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

        private void Awake()
        {
            SaveManager sm = FindObjectOfType<SaveManager>();
            if (sm == null)
                return;
            sm.OnPreSerialization.AddListener(PreSerialization);
            sm.OnPreDeserialization.AddListener(PreDeserialization);
            sm.OnPostDeserialization.AddListener(PostDeserialization);
        }

        private void OnDestroy()
        {
            SaveManager sm = FindObjectOfType<SaveManager>();
            if (sm == null)
                return;
            sm.OnPreSerialization.RemoveListener(PreSerialization);
            sm.OnPreDeserialization.RemoveListener(PreDeserialization);
            sm.OnPostDeserialization.RemoveListener(PostDeserialization);
        }

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
            List<RaycastHit> objects = new List<RaycastHit>();


            // Add proper objects to list
            foreach (RaycastHit hit in hits) 
            {
                IInteractable interact = hit.transform.gameObject.GetComponent<IInteractable>();
                Collider collider = hit.transform.gameObject.GetComponent<Collider>();

                if (interact != null || (!collider.isTrigger && collider.gameObject.layer != 7))
                    objects.Add(hit);
            }

            // Sort list from closest to farthest
            Queue<GameObject> hitQueue = SortByClosest(objects);

            // set hit object
            IInteractable closest = null;
            if (hitQueue.Count > 0)
            {
                closest = hitQueue.Peek().GetComponent<IInteractable>();
            }

            bool buttonDown = interact.ReadValue<float>() == 1;

            if (closest != null)
            {
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

        public Queue<GameObject> SortByClosest(List<RaycastHit> objs) 
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            while (objs.Count > 0) 
            {
                RaycastHit obj = GetClosest(objs);
                queue.Enqueue(obj.transform.gameObject);
                objs.Remove(obj);
            }
            return queue;
        }

        public RaycastHit GetClosest(List<RaycastHit> objs) 
        {
            RaycastHit toReturn = objs[0];
            float shortest = int.MaxValue;
            foreach (RaycastHit i in objs) 
            {
                float distance = i.distance;
                if (distance < shortest)
                {
                    shortest = distance;
                    toReturn = i;
                }
            }
            return toReturn;
        }

        public void PreSerialization()
        {
            return;
        }

        public void PreDeserialization()
        {
            UpClickQueue.Clear();
            Previous = null;
            tt.Clear();
            return;
        }

        public void PostDeserialization()
        {
            return;
        }
    }
}