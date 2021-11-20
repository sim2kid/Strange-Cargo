using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genetics {
    public class BoneToPick : MonoBehaviour
    {
        public string BoneOffset;
        public Vector3 Offset;

        public void Populate(string bone, Vector3 offset) 
        {
            BoneOffset = bone;
            Offset = offset;
        }
    }
}