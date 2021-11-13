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
        public bool HideOnZero = true;

        public IValue Value;

        private float unitValue => (Value.Read() - Value.MinValue) / (Value.MaxValue - Value.MinValue);


        void Update()
        {
            if (ObjToDisplace != null && Value != null)
            {
                if (unitValue <= 0 && HideOnZero)
                {
                    ObjToDisplace.SetActive(false);
                }
                else
                {
                    ObjToDisplace.SetActive(true);
                }

                ObjToDisplace.transform.localPosition = DisplaceTo * unitValue;
            }
        }
    }
}