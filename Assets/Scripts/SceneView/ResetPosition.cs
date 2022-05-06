using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InSceneView
{
    public class ResetPosition : MonoBehaviour
    {
        public Vector3 originalPosition;
        public Quaternion originalRotation;


        void Start()
        {
            originalPosition = transform.position;
            originalRotation = transform.rotation;
        }

        public void PositionReset()
        {
            transform.position = originalPosition;
            transform.rotation = originalRotation;
        }
    }
}