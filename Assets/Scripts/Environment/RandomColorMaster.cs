using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;


namespace Environment
{
    [System.Serializable]
    public class RandomColorMaster : MonoBehaviour
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

            RandomColor[] list = this.GetComponentsInChildren<RandomColor>();
            foreach (RandomColor c in list)
                c.Color = Color;
        }
    }
}