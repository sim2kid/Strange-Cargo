using PersistentData.Loading;
using PersistentData.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Environment
{
    [RequireComponent(typeof(Renderer))]
    public class RandomColor : MonoBehaviour, PersistentData.Component.ISaveable
    {
        public ColorData colorData;

        public string ColorPalette = "Data/ColorPalette/default";

        public ISaveData saveData { get => colorData; set { colorData = (ColorData)value; } }

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(colorData._guid))
            {
                colorData._guid = System.Guid.NewGuid().ToString();
            }
        }

        public void Awake()
        {
            if (string.IsNullOrWhiteSpace(colorData.Color))
            {
                colorData.Color = RandomColorPicker.ColorToHex(
                    RandomColorPicker.RetriveRandomColor(
                        RandomColorPicker.DefaultSeperationChar, ColorPalette));
            }
        }

        public void NewColor() 
        {
            colorData.Color = RandomColorPicker.ColorToHex(
                       RandomColorPicker.RetriveRandomColor(
                           RandomColorPicker.DefaultSeperationChar, ColorPalette));
            this.GetComponent<Renderer>().material.color = RandomColorPicker.HexToColor(colorData.Color);
        }

        public void PostDeserialization()
        {
            return;
        }

        public void PreDeserialization()
        {
            return;
        }

        public void PreSerialization()
        {
            return;
        }

        private void Start()
        {
            this.GetComponent<Renderer>().material.color = RandomColorPicker.HexToColor(colorData.Color);
        }
    }
}