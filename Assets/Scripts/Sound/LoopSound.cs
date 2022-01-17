using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound
{
    [System.Serializable]
    public class LoopSound : OldBasicSound
    {
        private int index = 0;
        public override AudioClip Clip 
        {
            get 
            {
                if (index >= 0 && index < _clipPool.Count)
                    return _clipPool[index];
                else
                    return null;
            }  
        }
        public void NextIndex() 
        {
            index++;
            if (index > _clipPool.Count)
                index = 0;
        }
        public void SetIndex(int newIndex) 
        {
            index = Mathf.Clamp(newIndex,0, Count()-1);
        }
        public int Count() 
        {
            return _clipPool.Count;
        }
    }
}