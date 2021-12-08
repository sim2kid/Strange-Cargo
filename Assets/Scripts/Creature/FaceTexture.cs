using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature
{
    public class FaceTexture : MonoBehaviour
    {
        public GameObject Eyes;
        public GameObject Mouth;

        private void Start()
        {
            Eyes.GetComponent<Renderer>().material.shader = Shader.Find("Shader Graphs/Face");
            Mouth.GetComponent<Renderer>().material.shader = Shader.Find("Shader Graphs/Face");

            Texture2D empty = Resources.Load<Texture2D>("Face/Empty");
            SetExpression(empty, empty);

        }

        /// <summary>
        /// Set the Eyes/Mouth texture
        /// </summary>
        /// <param name="eyeTex"></param>
        /// <param name="mouthTex"></param>
        public void SetExpression(Texture2D eyeTex = null, Texture2D mouthTex = null)
        {
            if (eyeTex != null)
                Eyes.GetComponent<Renderer>().material.SetTexture("_MainTex", eyeTex);
            if (mouthTex != null)
                Mouth.GetComponent<Renderer>().material.SetTexture("_MainTex", mouthTex);
        }
    }
}