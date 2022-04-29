using Interaction;
using PersistentData.Component;
using PersistentData.Saving;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Creature
{

    public class Mouth : MonoBehaviour
    {
        [SerializeField]
        Vector3 OffsetFromHeadbone = new Vector3(0.24f, 0.1f, -0.49f);

        public IHoldable Holding { get; private set; }
        public StringListData data;
        public ISaveData saveData { get => data; set => data = (StringListData)value; }

        private CreatureController creature;

        private bool enableGravityOnDrop;

        public bool HasObj => Holding != null;

        private void Awake()
        {
            // Since this part is autogened, it needs a static guid.
            data.GUID = "9913b6c1-1864-4f0c-bfd2-ccb08ecbbf45";
            Holding = null;
        }

        public void Populate(CreatureController controller) 
        {
            creature = controller;
        }

        void Start()
        {
            enableGravityOnDrop = false;
        }

        private void Update()
        {
            if (Holding == null)
                return;
            if (Holding.ToString().Equals("null"))
            {
                Holding = null;
                return;
            }
            if (Holding is MonoBehaviour)
            {
                GameObject obj = ((MonoBehaviour)Holding).gameObject;
                Vector3 ful = (transform.forward * OffsetFromHeadbone.x) + (transform.up * OffsetFromHeadbone.y) + (transform.right * OffsetFromHeadbone.z);
                obj.transform.position = transform.position + ful + Holding.PositionOffset;
                obj.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + Holding.RotationOffset);
            }
        }

        public void PickUp(IHoldable obj)
        {
            if (Holding != null)
            {
                LetGo();
            }
            Holding = obj;
            if (Holding is MonoBehaviour)
            {
                GameObject gameObject = ((MonoBehaviour)Holding).gameObject;
                Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    enableGravityOnDrop = rb.useGravity;
                    rb.useGravity = false;
                }
                var colliders = gameObject.GetComponentsInChildren<Collider>();
                foreach (Collider collider in colliders)
                {
                    collider.enabled = false;
                }
            }
            //if (player != null)
            //{
            //    //player.Footsteps.OnStep.AddListener(Holding.Shake);
            //    player.HeadMovement.OnJolt.AddListener(Holding.Shake);
            //}
        }
        public void LetGo()
        {
            if (Holding != null)
            {
                if (creature == null)
                    return;
                //if (player.HeadMovement != null)
                //    player.HeadMovement.OnJolt.RemoveListener(Holding.Shake);
                if (Holding is MonoBehaviour)
                {
                    GameObject gameObject = ((MonoBehaviour)Holding).gameObject;
                    Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.useGravity = enableGravityOnDrop;
                    }
                    var colliders = gameObject.GetComponentsInChildren<Collider>();
                    foreach (Collider collider in colliders)
                    {
                        collider.enabled = true;
                    }
                }
                Holding.PutDown();
            }
            enableGravityOnDrop = false;
            Holding = null;
        }

        public void PreSerialization()
        {
            if (data.StrList == null)
                data.StrList = new List<string>();
            data.StrList.Clear();
            // If the hand has a Saveable
            MonoBehaviour mb = (MonoBehaviour)Holding;
            if (mb != null)
            {
                GameObject obj = mb.gameObject;
                Saveable savMaster = obj.GetComponentInParent<Saveable>();
                if (savMaster != null)
                {
                    // Save the guid to the data.StrList
                    data.StrList.Add(savMaster.Data.GUID);
                }
            }
            return;
        }

        public void PreDeserialization()
        {
            LetGo();
        }

        public void PostDeserialization()
        {
            if (data.StrList.Count > 0)
            {
                string objGuid = data.StrList[0];
                // Put the saved object into the hand
                List<Saveable> saveablesInScene = FindObjectsOfType<Saveable>(true).ToList();
                Saveable saveable = saveablesInScene.FirstOrDefault(x => x.Data?.GUID?.Equals(objGuid) == true);
                if (saveable != null)
                {
                    IHoldable h = saveable.GetComponent<IHoldable>();
                    if (h != null)
                    {
                        PickUp(h);
                    }
                }
            }
        }
    }
}