using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sound.Structure;

namespace Sound.Player
{
    [RequireComponent(typeof(AudioPlayer))]
    public class SwitchController : MonoBehaviour
    {
        protected AudioPlayer ap;
        SwitchContainer container;
        private void Start()
        {
            ap = GetComponent<AudioPlayer>();
        }

        public bool HasSwitchContainer() 
        {
            return ap.Container is SwitchContainer;
        }
        public bool SetSelection(int selection) 
        {
            if (HasSwitchContainer()) 
            {
                if (container == null)
                {
                    container = ap.Container as SwitchContainer;
                }
                container.Selection = selection;
                return true;
            }
            return false;
        }
    }
}