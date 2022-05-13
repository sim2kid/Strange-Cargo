using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Environment.Tub
{
    [RequireComponent(typeof(InSceneView.ResetPosition))]
    public class Shampoo : MonoBehaviour, IReplaceable
    {
        InputAction StationaryUse;
        [SerializeField]
        GameObject soapBubblePrefab;
        List<GameObject> soapBubbles;
        int bubbleIndex = 0;
        GameObject soapBubbleParent;
        [SerializeField]
        GameObject spawnSoapLoaction;
        float cooldown = 0;
        [SerializeField]
        Bathtub tub;

        [SerializeField]
        float spawnSpread = 0.01f;

        public GameObject GameObject => gameObject;
        public Vector3 rotationOffset => Vector3.zero;

        public InSceneView.ResetPosition reset;

        [SerializeField]
        int totalActive = 0;

        public bool inHand;

        void Start()
        {
            reset = GetComponent<InSceneView.ResetPosition>();
            inHand = false;
            PlayerInput input = GameObject.FindObjectOfType<PlayerInput>();
            if (input == null)
            {
                Console.LogError($"Scene is missing a Player Input Component.");
                Destroy(this);
                return;
            }
            StationaryUse = input.actions["StationaryUse"];

            soapBubbleParent = new GameObject("Soap Bubbles");
            soapBubbleParent.transform.SetParent(this.transform.parent);

            soapBubbles = new List<GameObject>();
            for (int i = 0; i < 100; i++) 
            {
                var obj = Instantiate(soapBubblePrefab, soapBubbleParent.transform, false) as GameObject;
                soapBubbles.Add(obj);
                obj.SetActive(false);
            }
            tub = FindObjectOfType<Bathtub>();
        }

        private void OnDestroy()
        {
            Destroy(soapBubbleParent);
        }

        private void SpawnBubble() 
        {
            GameObject NextBubble = soapBubbles[bubbleIndex];



            NextBubble.transform.position = spawnSoapLoaction.transform.position + 
                new Vector3(Random.Range(-spawnSpread, spawnSpread), Random.Range(-spawnSpread, spawnSpread), 0);
            NextBubble.transform.rotation = spawnSoapLoaction.transform.rotation;

            if (NextBubble.activeSelf)
            {
                NextBubble.GetComponent<SoapBubble>().OnEnable();
            }
            else
            {
                NextBubble.SetActive(true);
            }

            bubbleIndex++;
            if (bubbleIndex >= soapBubbles.Count) 
            {
                bubbleIndex -= soapBubbles.Count;
            }
        }

        private void Update()
        {
            bool isHeld = inHand && StationaryUse.ReadValue<float>() > 0.5f;
            if (isHeld)
            {
                cooldown -= Time.deltaTime;
                if (cooldown < 0) 
                {
                    cooldown -= 0.2f;
                    SpawnBubble();
                }
                
            }

            totalActive = 0;
            foreach (var obj in soapBubbles) 
            {
                if (obj.activeSelf) 
                {
                    totalActive++;
                }
            }
        }

        public void Pickup()
        {
            inHand = true;
        }

        public void Putdown()
        {
            inHand = false;
            reset.LerpHome(2f);
        }
    }
}