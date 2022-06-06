using Sound.Player;
using Sound.Structure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Environment.Tub
{
    [RequireComponent(typeof(InSceneView.ResetPosition))]
    public class Showerhead : MonoBehaviour, IReplaceable
    {
        public InSceneView.ResetPosition reset;

        public GameObject GameObject => gameObject;

        [SerializeField]
        private Vector3 _rotationOffset;
        [SerializeField]
        Bathtub tub;
        [SerializeField]
        GameObject waterBubblePrefab;
        List<GameObject> waterBubbles;
        int bubbleIndex = 0;
        GameObject waterBubbleParent;
        [SerializeField]
        GameObject spawnWaterLoaction;
        float cooldown = 0;

        [SerializeField]
        float spawnSpread = 0.05f;

        [SerializeField]
        int totalActive = 0;

        AudioPlayer ap;

        public Vector3 rotationOffset => _rotationOffset;

        public bool active;
        InputAction Use;

        public void Pickup()
        {
            active = true;
            ((SwitchContainer)(ap.Container)).Selection = 0;
            ap.Play();
        }

        public void Putdown()
        {
            reset.LerpHome(2f);
            active = false;
            if (ap.IsPlaying)
                ap.Stop();
            ((SwitchContainer)(ap.Container)).Selection = 1;
            ap.Play(true);
            tub.Shake();
        }

        void Start()
        {
            reset = GetComponent<InSceneView.ResetPosition>();
            ap = GetComponent<AudioPlayer>();
            active = false;
            PlayerInput input = GameObject.FindObjectOfType<PlayerInput>();
            Use = input.actions["StationaryUse"];
            tub = FindObjectOfType<Bathtub>();

            waterBubbleParent = new GameObject("Water Bubbles");
            waterBubbleParent.transform.SetParent(this.transform.parent);

            waterBubbles = new List<GameObject>();
            for (int i = 0; i < 100; i++)
            {
                var obj = Instantiate(waterBubblePrefab, waterBubbleParent.transform, false) as GameObject;
                waterBubbles.Add(obj);
                obj.SetActive(false);
            }
        }

        private void SpawnBubble()
        {
            GameObject NextBubble = waterBubbles[bubbleIndex];

            NextBubble.transform.position = spawnWaterLoaction.transform.position +
                new Vector3(Random.Range(-spawnSpread, spawnSpread), Random.Range(-spawnSpread, spawnSpread), 0);
            NextBubble.transform.rotation = spawnWaterLoaction.transform.rotation;

            if (NextBubble.activeSelf)
            {
                NextBubble.GetComponent<WaterBubble>().OnEnable();
            }
            else
            {
                NextBubble.SetActive(true);
            }

            bubbleIndex++;
            if (bubbleIndex >= waterBubbles.Count)
            {
                bubbleIndex -= waterBubbles.Count;
            }
        }


        void Update()
        {
            bool isHeld = active;// && Use.ReadValue<float>() > 0.5f;
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
            foreach (var obj in waterBubbles)
            {
                if (obj.activeSelf)
                {
                    totalActive++;
                }
            }
        }

    }
}