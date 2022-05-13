using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utility.Input;

namespace Environment.Tub
{
    public class Bathtub : MonoBehaviour
    {
        [SerializeField]
        Transform destination;
        [SerializeField]
        GameObject cursor;

        [SerializeField]
        float tubDistance = 2f;

        Creature.CreatureController Creature;

        private PlayerInput input;
        private InputAction look;
        private InputAction use;
        private InputAction drop;
        private InputContext context;

        [SerializeField]
        public GameObject UI;

        [SerializeField]
        public Slider shampoo;
        [SerializeField]
        public Slider Hygiene;

        private Vector2 screenLoc;

        IReplaceable inHand;

        public bool active;
        public float Soap;
        private bool awaitingCreature;
        public void SetActive() 
        {
            active = true;
            FindObjectOfType<Player.PlayerController>().PlayerModel.SetActive(false);
            float mindis = float.MaxValue;
            foreach (var creature in Utility.Toolbox.Instance.CreatureList) 
            {
                var dis = Vector3.Distance(destination.position, creature.transform.position);
                if (dis < mindis) 
                {
                    Creature = creature;
                }
            }
            if (Creature == null)
                SetInactive();

            Creature.BrainDead = true;
            Creature.Move.MoveTo(destination.position);
            awaitingCreature = true;

            screenLoc = new Vector2(Screen.width / 2, Screen.height / 2);
            cursor.SetActive(true);
            UI.SetActive(true);
        }

        public void SetInactive() 
        {
            active = false;
            FindObjectOfType<Player.PlayerController>().PlayerModel.SetActive(true);
            if (Creature != null) 
            {
                Creature.BrainDead = false;
            }
            Creature = null;
            awaitingCreature = false;
            cursor.SetActive(false);
            if (inHand != null)
            {
                inHand.Putdown();
                inHand = null;
            }
            UI.SetActive(false);
        }

        public float ModSoap(float amount) 
        {
            float final = Mathf.Clamp(Soap + amount, shampoo.minValue, shampoo.maxValue); 
            float delta = Soap - final;
            Soap = final;
            return Mathf.Abs(delta);
        }

        public void ModWater(float amount) 
        {
            float change = ModSoap(-amount);
            Creature.ProcessNeedChange(new Creature.Stats.Needs() { Hygiene = change });
        }

        private void onCreatureArrive() 
        {
            Creature.transform.rotation = Quaternion.Lerp(Creature.transform.rotation, destination.rotation, Time.deltaTime * 2f);
        }

        public void Shake() 
        {
            Creature.AnimationBool("Wiggle", true);
            Invoke("StopShake", 0.8f);
        }

        private void StopShake()
        {
            Creature.AnimationBool("Wiggle", false);
        }

        void Start()
        {
            input = FindObjectOfType<PlayerInput>();
            look = input.actions["StationaryLook"];
            use = input.actions["StationaryUse"];
            drop = input.actions["StationaryDrop"];

            context = FindObjectOfType<InputContext>();

            if (inHand != null) 
            {
                inHand.Putdown();
            }
            inHand = null;
            UI.SetActive(false);
        }

        private void OnDrawGizmos()
        {
            var point = new Vector3(screenLoc.x, screenLoc.y, 0.5f);
            var ray = Camera.main.ScreenPointToRay(point);
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * 3f);
        }

        void Update()
        {
            if (awaitingCreature) 
            {
                var distance = Vector3.Distance(destination.position, Creature.transform.position);
                if (distance < 1f) {
                    onCreatureArrive();
                    if (Quaternion.Angle(Creature.transform.rotation, destination.transform.rotation) < 10)
                    {
                        awaitingCreature = false;
                    }
                }
            }
            if (active)
            {
                // Set screen position
                if (context.CurrentScheme == Scheme.KeyboardMouse)
                {
                    screenLoc = look.ReadValue<Vector2>();
                }
                else
                {
                    screenLoc += look.ReadValue<Vector2>();
                }
                screenLoc = new Vector2(Mathf.Clamp(screenLoc.x, 0, Screen.width), Mathf.Clamp(screenLoc.y, 0, Screen.height));

                var point = new Vector3(screenLoc.x, screenLoc.y, 0.5f);

                var worldPos = Camera.main.ScreenToWorldPoint(point);
                if (cursor.activeSelf)
                {
                    cursor.transform.position = worldPos;
                }

                // On click with empty hand
                if (use.triggered && inHand == null)
                {
                    var ray = Camera.main.ScreenPointToRay(point);
                    var hits = Physics.RaycastAll(ray, 3);

                    IReplaceable ItemOfInterest = null;
                    foreach (var hit in hits)
                    {
                        var item = hit.transform.gameObject.GetComponent<IReplaceable>();
                        if (item != null)
                        {
                            ItemOfInterest = item;
                            inHand = ItemOfInterest;
                            inHand.Pickup();
                            cursor.SetActive(false);
                            break;
                        }
                    }
                }
                else if(drop.triggered)
                {
                    inHand.Putdown();
                    inHand = null;
                    cursor.SetActive(true);
                }

                if (inHand != null)
                {
                    point.z = tubDistance;
                    Vector3 hand = Camera.main.ScreenToWorldPoint(point);
                    //hand.z = destination.position.z;
                    var tf = inHand.GameObject.transform;
                    tf.position = Vector3.Lerp(tf.position, hand, Time.deltaTime * 5f);



                    tf.rotation = Quaternion.Lerp(tf.rotation, Quaternion.Euler(inHand.rotationOffset + cursor.transform.rotation.eulerAngles), Time.deltaTime);
                }

                shampoo.value = Soap;
                Hygiene.value = Creature.needs.Hygiene;
            }
        }
    }
}