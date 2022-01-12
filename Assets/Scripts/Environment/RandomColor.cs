using PersistentData.Component;
using PersistentData.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Environment
{
    [RequireComponent(typeof(Renderer))]
    public class RandomColor : MonoBehaviour, ISaveable
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
                NewColor();
            }
        }

        public void NewColor() 
        {
            colorData.Color = RandomColorPicker.ColorToHex(
                       RandomColorPicker.RetriveRandomColor(
                           RandomColorPicker.DefaultSeperationChar, ColorPalette));
            Renderer renderer = this.GetComponent<Renderer>();
            if (renderer != null)
                foreach (UnityEngine.Material material in renderer.materials)
                    material.color = RandomColorPicker.HexToColor(colorData.Color);
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