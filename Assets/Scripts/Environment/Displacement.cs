using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataType;

namespace Environment
{
    public class Displacement : MonoBehaviour
    {
        [SerializeField]
        public GameObject ObjToDisplace;
        [SerializeField]
        public Vector3 DisplaceTo = Vector3.zero;
        [SerializeField]
        public bool SmoothTransition = true;
        [SerializeField]
        public float SmoothingRate = 2f;
        [SerializeField]
        public bool HideOnZero = true;

        public IValue Value;

        private float unitValue => (Value.Read() - Value.MinValue) / (Value.MaxValue - Value.MinValue);

        private void Start()
        {
            if (ObjToDisplace != null && Value != null)
            {
                ObjToDisplace.transform.localPosition += DisplaceTo * unitValue;
            }
        }

        private void Update()
        {
            if (ObjToDisplace != null && Value != null)
            {
                

                Vector3 desiredPosition = DisplaceTo * unitValue;
                float smooth = SmoothingRate * Time.deltaTime;
                Vector3 ChangeInPosition = desiredPosition - ObjToDisplace.transform.localPosition;
                if (SmoothTransition)
                {
                    ObjToDisplace.transform.localPosition += ChangeInPosition * smooth;
                }
                else 
                {
                    ObjToDisplace.transform.localPosition += desiredPosition;
                }

                if (unitValue <= 0 && HideOnZero && ChangeInPosition.magnitude <= DisplaceTo.magnitude * 0.01f )
                {
                    ObjToDisplace.SetActive(false);
                }
                else
                {
                    ObjToDisplace.SetActive(true);
                }
            }
        }
    }
}