using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InSceneView
{
    public class ResetPosition : MonoBehaviour
    {
        public Vector3 originalPosition;
        public Quaternion originalRotation;
        public Transform originalParent;


        void Start()
        {
            originalPosition = transform.position;
            originalRotation = transform.rotation;
            originalParent = transform.parent;
        }

        public void PositionReset()
        {
            transform.position = originalPosition;
            transform.rotation = originalRotation;
            transform.SetParent(originalParent);
        }

        Vector3 startPosition = Vector3.zero;
        Quaternion startRotation = Quaternion.identity;
        float startTime = 0, time = 0;

        public void LerpHome(float time) 
        {
            startTime = time;
            this.time = startTime;
            startPosition = transform.position;
            startRotation = transform.rotation;
        }

        private void Update()
        {
            if (time > 0) 
            {
                time -= Time.deltaTime;
                transform.position = Vector3.Lerp(originalPosition, startPosition, time / startTime);
                transform.rotation = Quaternion.Lerp(originalRotation, startRotation, time / startTime);
                if (time <= 0) 
                {
                    PositionReset();
                }
            }
        }
    }
}