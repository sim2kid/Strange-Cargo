using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Environment
{
    [System.Serializable]
    public class RandomColor : MonoBehaviour
    {
        public string color;

        public void Awake()
        {
            if (string.IsNullOrWhiteSpace(color)) 
            {
                color = RandomColorPicker.ColorToHex(
                    RandomColorPicker.RetriveRandomColor(
                        RandomColorPicker.DefaultSeperationChar, "Data/ColorPalette/default"));
            }
        }

        private void OnEnable()
        {
            this.GetComponent<Renderer>().material.color = RandomColorPicker.HexToColor(color);
        }
    }
}