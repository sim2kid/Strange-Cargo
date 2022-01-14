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
        StringListData data;
        public ISaveData saveData { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        private PlayerController player;
        private Utility.ToolTip tt;

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
            StartCoroutine(LateStart(1));
        }

        IEnumerator LateStart(float waitTime) 
        {
            yield return new WaitForSeconds(waitTime);
            player = Utility.Toolbox.Instance.Player;
            player.GlobalInteraction.PrimaryEvent.AddListener(LetGo);
            player.GlobalInteraction.UseEvent.AddListener(Use);
            player.GlobalInteraction.ThrowEvent.AddListener(Throw);
            tt = Utility.Toolbox.Instance.ToolTip;
        }

        private void Update()
        {
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
            if(tt != null)
                tt.HoldText = Holding.HoldText;
            if (player != null)
            {
                player.Footsteps.OnStep.AddListener(Holding.Shake);
                player.HeadMovement.OnJolt.AddListener(Holding.Shake);
            }
        }
        public void LetGo() 
        {
            if (Holding != null)
            {
                player.Footsteps.OnStep.RemoveListener(Holding.Shake);
                player.HeadMovement.OnJolt.RemoveListener(Holding.Shake);
                Holding.PutDown();
            }
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
                List<Saveable> saveablesInScene = FindObjectsOfType<Saveable>(true).ToList(); ;
                Saveable saveable = saveablesInScene.Find(x => x.Data.GUID.Equals(objGuid));
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