using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Environment
{
    [System.Serializable]
    [RequireComponent(typeof(Renderer))]
    public class RandomColor : MonoBehaviour
    {
        public string Color;
        public string ColorPalette = "Data/ColorPalette/default";

        public void Awake()
        {
            if (string.IsNullOrWhiteSpace(Color))
            {
                Color = RandomColorPicker.ColorToHex(
                    RandomColorPicker.RetriveRandomColor(
                        RandomColorPicker.DefaultSeperationChar, ColorPalette));
            }
        }

        public void NewColor() 
        {
            Color = RandomColorPicker.ColorToHex(
                       RandomColorPicker.RetriveRandomColor(
                           RandomColorPicker.DefaultSeperationChar, ColorPalette));
            this.GetComponent<Renderer>().material.color = RandomColorPicker.HexToColor(Color);
        }

        private void Start()
        {
            this.GetComponent<Renderer>().material.color = RandomColorPicker.HexToColor(Color);
        }
    }
}