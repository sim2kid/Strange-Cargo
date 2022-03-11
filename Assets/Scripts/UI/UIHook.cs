using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIHook : MonoBehaviour
    {
        UISound uisound;

        void Start()
        {
            uisound = FindObjectOfType<UISound>();
        }

        public void Play(SFX sound)
        {
            uisound.Play(sound);
        }

        public static void PlaySound(SFX sound)
        {
            FindObjectOfType<UISound>().Play(sound);
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