using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment.Tub
{
    [RequireComponent(typeof(InSceneView.ResetPosition))]
    public class Showerhead : MonoBehaviour
    {
        public InSceneView.ResetPosition reset;

        void Start()
        {
            reset = GetComponent<InSceneView.ResetPosition>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}