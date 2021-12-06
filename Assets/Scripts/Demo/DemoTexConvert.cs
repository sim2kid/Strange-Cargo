using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextureConverter {
    [RequireComponent(typeof(MaterialConversion))]
    public class DemoTexConvert : MonoBehaviour
    {
        MaterialConversion con;

        public float Progress = 0;
        public bool ConvertButton = false;
        public int speed = 50000;

        void Start()
        {
            con = GetComponent<MaterialConversion>();
            Progress = con.Report();
            con.CONVERSION_SPEED = speed;
        }

        // Update is called once per frame
        void Update()
        {
            Progress = con.Report();
            if (ConvertButton && Progress == 1) 
            {
                con.Convert();
                ConvertButton = false;
            }
        }
    }
}