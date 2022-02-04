using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interaction;
using UnityEngine.Events;
using PersistentData.Component;
using System.Linq;

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

        [SerializeField]
        private bool CanPlaceOnFloor = true;
        private float MaxFloorAngle = 15.0f;
        [SerializeField]
        private bool CanPlaceOnWall = false;
        private float MaxWallAngle = 15.0f;

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
                hologram.transform.position = hitPos + (0.1f * hitInfo.normal);
                hologram.transform.localScale = objectToPlace.transform.localScale;
                var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
                hologram.transform.rotation = slopeRotation;

                foreach (Renderer render in hologram.GetComponentsInChildren<Renderer>()) 
                {
                    foreach (Material material in render.materials) 
                    {
                        if (IsValidLocation())
                        {
                            material.color = validColor;
                        }
                        else
                        {
                            material.color = invalidColor;
                        }
                    }
                }
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

        private bool IsValidLocation() 
        {
            float hologramAngle = Vector3.Angle(Vector3.up, hologram.transform.up);

            //var AllColliders = hologram.GetComponentsInChildren<Collider>();
            //var Renderers = hologram.GetComponentsInChildren<Renderer>();
            //// No collisions
            //foreach (Renderer render in Renderers)
            //{
            //    Collider[] colliders = render.GetComponents<Collider>();
            //    if (colliders == null)
            //        continue;
            //    foreach (Collider collider in colliders)
            //    {
            //        if (collider == null)
            //            continue;
            //        if (!collider.enabled)
            //            continue;
            //        if (collider.isTrigger)
            //            continue;
            //        Vector3 size = collider.bounds.size;
            //        float maxSize = Mathf.Max(size.x, size.y, size.z);
            //        var collisions = Physics.OverlapSphere(collider.bounds.center, maxSize, canPlaceOn);
            //        foreach (Collider other in collisions)
            //        {
            //            if (AllColliders.Contains(other))
            //                continue;
            //            if (other.isTrigger || !other.enabled)
            //                continue;
            //            if (Physics.ComputePenetration(collider, collider.transform.position, collider.transform.rotation,
            //                other, other.transform.position, other.transform.rotation,
            //                out Vector3 direction, out float distance))
            //            {
            //                if (Vector3.Angle(direction, hologram.transform.up) > 180 - MaxFloorAngle)
            //                {
            //                    continue;
            //                }
            //                return false;
            //            }
            //        }
            //    }
            //}


            // Valid Angle
            if (CanPlaceOnFloor && hologramAngle < MaxFloorAngle) 
            {
                return true;
            }
            if (CanPlaceOnWall && Mathf.Abs(hologramAngle - 90) < MaxWallAngle) 
            {
                return true;
            }
            return false;
        }

        protected override void Start()
        {
            base.Start();
            OnPickup.AddListener(onPickup);
            OnPutDown.AddListener(onDrop);

            Physics.IgnoreLayerCollision(10, canPlaceOn, true);
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

            Component[] components = hologram.GetComponentsInChildren<Component>();
            foreach (Component component in components) 
            {
                if (component is Transform || component is Collider || component is Renderer || component is MeshFilter)
                {
                    continue;
                }
                Destroy(component);
            }

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