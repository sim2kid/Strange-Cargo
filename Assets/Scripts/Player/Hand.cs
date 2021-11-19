using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interaction;

namespace Player
{
    public class Hand : MonoBehaviour
    {

        public IHoldable Holding { get; private set; }

        private PlayerController player;

        private void OnEnable()
        {
            Holding = null;
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
        }

        private void Update()
        {
            if (Holding is IUseable)
                ((IUseable)Holding).HoldUpdate();
        }

        public void Use() 
        {
            if (Holding is IUseable)
                ((IUseable)Holding).Use();
        }

        public void PickUp(IHoldable obj) 
        {
            if (Holding != null)
                LetGo();
            Holding = obj;
        }
        public void LetGo() 
        {
            if(Holding != null)
                Holding.PutDown();
            Holding = null;
        }
    }
}