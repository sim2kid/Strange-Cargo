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

        [SerializeField]
        private float offsetFromSurface = 0f;

        private GameObject hologram;
        private Hologram gramInfo;

        [SerializeField]
        string useString;
        public string UseText => useString;

        bool beingDestroyed = false;

        public void HoldUpdate()
        {
            if (beingDestroyed)
                return;

            if (Utility.Toolbox.Instance.Player.InputState != InputState.Build) 
            {
                Utility.Toolbox.Instance.Player.InputState = InputState.Build;
            }

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
                hologram.transform.position = hitPos + ((offsetFromSurface + gramInfo.Offset) * hitInfo.normal);
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
                    Destroy(hologram);
                    hologram = null;
                    gramInfo = null;
                }
            }

        }

        private bool IsValidLocation() 
        {
            if (hologram == null)
            {
                return false;
            }
            if (!hologram.scene.IsValid()) 
            {
                return false;
            }

            float hologramAngle = Vector3.Angle(Vector3.up, hologram.transform.up);

            // Check to make sure there are no collisions
            var collisions = Physics.OverlapBox(gramInfo.Center, gramInfo.HalfExtents, hologram.transform.rotation, canPlaceOn);

            foreach (Collider collider in collisions) 
            {
                if (collider.enabled == true && !collider.isTrigger) 
                {
                    return false;
                }
            }

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
            if (beingDestroyed)
                return;
            if (hologram != null)
            {
                if (hologram.scene.IsValid())
                {
                    Destroy(hologram);
                }
            }
            hologram = Instantiate(objectToPlace);

            // Destroy old components
            cleanComponenets();

            // Add hologram components
            gramInfo = hologram.AddComponent<Hologram>();

            foreach(Renderer renderer in hologram.GetComponentsInChildren<Renderer>())
                foreach (Material m in renderer.materials)
                    m.shader = hologramShader;
        }

        private void cleanComponenets() 
        {
            bool clean = false;
            for(int i = 0; i < 5 && !clean; i++) 
            {
                clean = true;
                Component[] components = hologram.GetComponentsInChildren<Component>();
                foreach (Component component in components)
                {
                    if (component is Transform || component is Renderer || component is MeshFilter)
                    {
                        continue;
                    }
                    clean = false;
                    Destroy(component);
                }
            }
        }

        private void onPickup() 
        {
            if (hologram != null)
                if (hologram.scene.IsValid())
                    return;
            createHolorgram();
            Utility.Toolbox.Instance.Player.InputState = InputState.Build;
        }

        private void onDrop()
        {
            if (hologram != null)
            {
                if (hologram.scene.IsValid())
                    Destroy(hologram);
                hologram = null;
                gramInfo = null;
            }
            Utility.Toolbox.Instance.Player.InputState = InputState.Default;
        }

        public void Use()
        {
            if (IsValidLocation())
            {
                GameObject obj = Instantiate(objectToPlace, hologram.transform.position, hologram.transform.rotation);
                beingDestroyed = true;
                Destroy(hologram);
                hologram = null;
                gramInfo = null;
                Destroy(gameObject);
            }
        }
        public void Mod1Use()
        {
            Use();
        }
    }
}