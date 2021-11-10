using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound
{
    [System.Serializable]
    public class LoopSound : BasicSound
    {
        private int index = 0;
        public override AudioClip Clip 
        {
            get 
            {
                return _clipPool[index]; 
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