using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interaction;
using PersistentData.Component;
using PersistentData.Saving;
using System.Linq;

namespace Player
{
    public class Hand : MonoBehaviour, ISaveable
    {

        public IHoldable Holding { get; private set; }
        public StringListData data;
        public ISaveData saveData { get => data; set => data = (StringListData)value; }

        private PlayerController player;
        private Utility.ToolTip tt;

        private bool enableGravityOnDrop;

        private void Awake()
        {
            Holding = null;
        }

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(data.GUID))
            {
                data.GUID = System.Guid.NewGuid().ToString();
            }
        }

        void Start()
        {
            enableGravityOnDrop = false;
        }

        void LateStart() 
        {
            player = Utility.Toolbox.Instance.Player;
            player.GlobalInteraction.DropEvent.AddListener(LetGo);
            player.GlobalInteraction.UseEvent.AddListener(Use);
            player.GlobalInteraction.Mod1UseEvent.AddListener(Mod1Use);
            player.GlobalInteraction.ThrowEvent.AddListener(Throw);
            tt = Utility.Toolbox.Instance.ToolTip;
        }

        private void Update()
        {
            if(player == null)
                LateStart();
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
                obj.transform.position = transform.position + Holding.PositionOffset;
                obj.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + Holding.RotationOffset);
            }
            if (Holding is IUseable && tt != null)
            {
                ((IUseable)Holding).HoldUpdate();
                tt.UseText = ((IUseable)Holding).UseText;
            }
        }

        public void Use() 
        {
            if (Holding is IUseable)
                ((IUseable)Holding).Use();
        }

        public void Mod1Use()
        {
            if (Holding is IUseable)
                ((IUseable)Holding).Mod1Use();
        }

        public void Throw()
        {
            if (Holding is IThrowable)
                ((IThrowable)Holding).Throw();
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
            }
            if(tt != null)
                tt.HoldText = Holding.HoldText;
            if (player != null)
            {
                //player.Footsteps.OnStep.AddListener(Holding.Shake);
                player.HeadMovement.OnJolt.AddListener(Holding.Shake);
            }
        }
        public void LetGo() 
        {
            if (Holding != null)
            {
                if (player == null)
                    return;
                //if(player.Footsteps != null)
                //    player.Footsteps.OnStep.RemoveListener(Holding.Shake);
                if (player.HeadMovement != null)
                    player.HeadMovement.OnJolt.RemoveListener(Holding.Shake);
                if (Holding is MonoBehaviour)
                {
                    GameObject gameObject = ((MonoBehaviour)Holding).gameObject;
                    Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.useGravity = enableGravityOnDrop;
                    }
                }
                Holding.PutDown();
            }
            enableGravityOnDrop = false;
            Holding = null;
            if (tt != null)
            {
                tt.HoldText = string.Empty;
                tt.UseText = string.Empty;
            }
            else 
            {
                tt = Utility.Toolbox.Instance.ToolTip;
            }
        }

        public void PreSerialization()
        {
            if(data.StrList == null)
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