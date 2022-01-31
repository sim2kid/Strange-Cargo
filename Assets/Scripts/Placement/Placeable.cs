using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interaction;
using UnityEngine.Events;

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
                if (hologram.scene.IsValid())
                {
                    hologram.transform.position = hitPos;
                    hologram.transform.localScale = objectToPlace.transform.localScale;
                }
                //Render hologram
            } 
            else 
            {
                hologram.transform.localScale = Vector3.zero;
            }

        }

        protected override void Start()
        {
            base.Start();
            OnPickup.AddListener(onPickup);
            OnPutDown.AddListener(onDrop);
        }

        private void onPickup() 
        {
            if (hologram != null)
                if (hologram.scene.IsValid())
                    return;
            hologram = Instantiate(objectToPlace);
            foreach (Material m in hologram.GetComponent<Renderer>().materials)
                m.shader = hologramShader;
        }

        private void onDrop()
        {
            if(hologram != null)
                if(hologram.scene.IsValid())
                    Destroy(hologram);
        }

        public void Use()
        {
            
        }
    }
}