using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genetics
{
    [System.Serializable]
    public class Face : MonoBehaviour
    {
        public bool IsEyes;
        public bool IsMouth;

        public void Populate(bool IsEyes, bool IsMouth)
        {
            this.IsEyes = IsEyes;
            this.IsMouth = IsMouth;
        }
    }
}