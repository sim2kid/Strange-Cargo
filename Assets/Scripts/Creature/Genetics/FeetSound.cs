using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genetics
{
    public class FeetSound : MonoBehaviour
    {
        public string Sound;
        public bool IsFrontFeet;

        public void Populate(string sound, bool isFront)
        {
            Sound = sound;
            IsFrontFeet = isFront;
        }
    }
}