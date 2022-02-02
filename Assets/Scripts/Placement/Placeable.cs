using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interaction;
using UnityEngine.Events;
using PersistentData.Component;

namespace Placement
{
    public class Placeable : Pickupable, IUseable
    {
        public UnityEvent OnUse;
        [SerializeField]
        private LayerMask canPlaceOn;
        [SerializeField]
        private float maxDistance = 10f;
        [SerializeField]
        private GameObject objectToPlace;

        [SerializeField]
        private Shader hologramShader;
        [SerializeField]
        private Color validColor = Color.green;
        [SerializeField]
        private Color invalidColor = Color.red;

        private GameObject hologram;

        [SerializeField]
        string useString;
        public string UseText => useString;

        public void HoldUpdate()
        {
            Transform player = Utility.Toolbox.Instance.Player.Eyes.transform;
            if (Physics.Raycast(player.position, player.forward, out RaycastHit hitInfo, maxDistance, canPlaceOn)) 
            {
                Vector3 hitPos = hitInfo.point;
                // Create hologram
                if (hologram == null)
                { 
                    createHolorgram();
                } 
                else if (!hologram.scene.IsValid())
                {
                    createHolorgram();
                }

                // Make sure hologram is in place
                hologram.transform.position = hitPos;
                hologram.transform.localScale = objectToPlace.transform.localScale;
                var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
                hologram.transform.rotation = slopeRotation;
            } 
            else if (hologram != null) 
            {
                if (hologram.scene.IsValid()) 
                {
                    hologram.transform.position = Vector3.zero;
                    hologram.transform.localScale = Vector3.zero;
                }
            }

        }

        protected override void Start()
        {
            base.Start();
            OnPickup.AddListener(onPickup);
            OnPutDown.AddListener(onDrop);
        }

        private void createHolorgram() 
        {
            if (hologram != null)
            {
                if (hologram.scene.IsValid())
                {
                    Destroy(hologram);
                }
            }
            hologram = Instantiate(objectToPlace);

            foreach(Saveable saveable in hologram.GetComponentsInChildren<Saveable>())
                Destroy(saveable);

            foreach (Collider collider in hologram.GetComponentsInChildren<Collider>())
            {
                collider.gameObject.layer = 10; // Holograms layer
            }

            foreach(Renderer renderer in hologram.GetComponentsInChildren<Renderer>())
                foreach (Material m in renderer.materials)
                    m.shader = hologramShader;
        }

        private void onPickup() 
        {
            if (hologram != null)
                if (hologram.scene.IsValid())
                    return;
            createHolorgram();
        }

        private void onDrop()
        {
            if (hologram != null)
            {
                if (hologram.scene.IsValid())
                    Destroy(hologram);
                hologram = null;
            }
        }

        public void Use()
        {
            
        }
    }
}