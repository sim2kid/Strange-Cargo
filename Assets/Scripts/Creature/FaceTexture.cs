using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature
{
    public class FaceTexture : MonoBehaviour
    {
        public GameObject Eyes;
        public GameObject Mouth;


        /// <summary>
        /// Set the Eyes/Mouth texture
        /// </summary>
        /// <param name="eyeTex"></param>
        /// <param name="mouthTex"></param>
        public void SetExpression(Texture2D eyeTex = null, Texture2D mouthTex = null)
        {
            if (eyeTex != null)
                Eyes.GetComponent<Renderer>().material.mainTexture = eyeTex;
            if (mouthTex != null)
                Mouth.GetComponent<Renderer>().material.mainTexture = mouthTex;
        }
    }
}