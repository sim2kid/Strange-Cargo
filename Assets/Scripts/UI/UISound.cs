using Sound.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class UISound : SwitchController
    {
        public bool SetSelection(SFX sfx) 
        {
            return SetSelection((int)sfx);
        }

        public void Play(SFX sound) 
        {
            SetSelection((int)sound);
            ap.Play(true);
        }

        public void PlaySelect() 
        {
            Play(SFX.SELECT);
        }

        public void PlayRollover()
        {
            Play(SFX.ROLLOVER);
        }

        public void PlayOpen() 
        {
            Play(SFX.OPEN);
        }

        public void PlayClose()
        {
            Play(SFX.CLOSE);
        }
        public void PlayBack()
        {
            Play(SFX.BACK);
        }
    }
}