using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature
{
    public class FaceTexture : MonoBehaviour
    {
        public Material Eyes;
        public Material Mouth;


        /// <summary>
        /// Set the Eyes/Mouth texture
        /// </summary>
        /// <param name="eyeTex"></param>
        /// <param name="mouthTex"></param>
        public void SetExpression(Texture2D eyeTex = null, Texture2D mouthTex = null)
        {
            if (eyeTex != null)
                Eyes.mainTexture = eyeTex;
            if (mouthTex != null)
                Mouth.mainTexture = mouthTex;
        }
    }
}