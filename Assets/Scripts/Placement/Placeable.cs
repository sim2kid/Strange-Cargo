using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interaction;
using UnityEngine.Events;
using PersistentData.Component;
using System.Linq;
using PersistentData.Saving;
using UnityEngine.InputSystem;

namespace Placement
{
    public class Placeable : Pickupable, IUseable, ISaveable
    {
        public UnityEvent OnUse;

        [SerializeField]
        string newObjResourceLocation;

        private GameObject objectToPlace => Resources.Load(prefabData.PrefabResourceLocation) as GameObject;
        
        public Shader HologramShader;
        public Color ValidColor = Color.green;
        public Color InvalidColor = Color.red;

        public float MaxPlaceDistance = 10f;

        public bool CanPlaceOnFloor = true;
        private float MaxFloorAngle = 15.0f;

        public bool CanPlaceOnWall = false;
        private float MaxWallAngle = 15.0f;

        private GameObject hologram;
        private Hologram gramInfo;

        private LayerMask canPlaceOn;
        private PrefabData prefabData { get => itemData.prefabData; set => itemData.prefabData = value; }
        private float rotation = 0;
        InputAction spin;
        PlayerInput playerInput;

        [SerializeField]
        private string useString;
        public string UseText => useString;

        bool beingDestroyed = false;

        [SerializeField]
        ItemData itemData;

        public override ISaveData saveData
        {
            get => itemData;
            set { itemData = (ItemData)value; }
        }

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(itemData.GUID))
            {
                itemData.GUID = System.Guid.NewGuid().ToString();
            }
        }

        public void HoldUpdate()
        {
            if (beingDestroyed)
                return;

            if (Utility.Toolbox.Instance.Player.InputState != InputState.Build) 
            {
                Utility.Toolbox.Instance.Player.InputState = InputState.Build;
            }

            Transform player = Utility.Toolbox.Instance.Player.Eyes.transform;
            if (Physics.Raycast(player.position, player.forward, out RaycastHit hitInfo, MaxPlaceDistance, canPlaceOn))
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

                // Update rotation data
                float change = spin.ReadValue<float>() * Time.deltaTime * 10f;
                rotation += change;
                if (rotation > 360)
                    rotation -= 360;
                if (rotation < 0)
                    rotation += 360;

                // Make sure hologram is in place
                hologram.transform.position = hitPos + ((gramInfo.Offset) * hitInfo.normal);
                hologram.transform.localScale = objectToPlace.transform.localScale;
                var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
                var floorRotation = Quaternion.AngleAxis(rotation, hitInfo.normal);

                hologram.transform.rotation = floorRotation * slopeRotation;

                foreach (Renderer render in hologram.GetComponentsInChildren<Renderer>()) 
                {
                    foreach (Material material in render.materials) 
                    {
                        if (IsValidLocation())
                        {
                            material.color = ValidColor;
                        }
                        else
                        {
                            material.color = InvalidColor;
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
            HologramShader = Shader.Find("Shader Graphs/Hologram");
            canPlaceOn = LayerMask.GetMask("Default", "Ground");

            playerInput = GameObject.FindObjectOfType<PlayerInput>();
            spin = playerInput.actions["Spin"];

            if (string.IsNullOrWhiteSpace(useString))
            {
                useString = "{spin} to Rotate and {use} to Place";
            }

            IgnorePhysics();
        }

        public void Hydrate(PrefabData prefabData) 
        {
            this.prefabData = prefabData;

            IgnorePhysics();
        }

        private static bool IsDataAllowed(PrefabData data) 
        {
            if (data == null)
                return true;

            var dataList = data.ExtraData;
            if (dataList != null)
            {
                foreach (var item in dataList) 
                {
                    if (item is ItemData)
                    {
                        // We don't want encapsulated Items!!
                        return false;
                    }
                }
            }
            return true;
        }

        private void IgnorePhysics() 
        {
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
                    m.shader = HologramShader;
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
            rotation = 0;
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
                if (string.IsNullOrWhiteSpace(prefabData.GUID)) 
                {
                    if (string.IsNullOrWhiteSpace(newObjResourceLocation)) 
                    {
                        Console.LogError($"Placeable, {name}, is unuseable due to lack of item to place. The prefab data was not filled out.");
                        return;
                    }
                    prefabData.GUID = System.Guid.NewGuid().ToString();
                    prefabData.ExtraData = new List<ISaveData>();
                    prefabData.PrefabResourceLocation = newObjResourceLocation;
                }
                GameObject obj = PersistentData.SaveManager.LoadPrefab(prefabData);
                obj.transform.position = hologram.transform.position;
                obj.transform.rotation = hologram.transform.rotation;
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

        public override void PreSerialization()
        {
            base.PreSerialization();
            itemData.rigidbodyData = rbData;
            if (!IsDataAllowed(prefabData)) 
            {
                prefabData = null;
            }
        }

        public override void PreDeserialization()
        {
            base.PreDeserialization();
        }

        public override void PostDeserialization()
        {
            rbData = itemData.rigidbodyData;
            base.PostDeserialization();
        }
    }
}