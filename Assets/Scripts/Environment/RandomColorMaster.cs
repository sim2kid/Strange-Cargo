using PersistentData.Component;
using PersistentData.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;


namespace Environment
{
    public class RandomColorMaster : MonoBehaviour, ISaveable
    {
        public ColorData colorData;
        public string ColorPalette = "Data/ColorPalette/default";

        public ISaveData saveData { get => colorData; set { colorData = (ColorData)value; } }


        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(colorData.GUID))
            {
                colorData.GUID = System.Guid.NewGuid().ToString();
            }
        }


        public void Awake()
        {
            if (string.IsNullOrWhiteSpace(colorData.Color))
            {
                NewColor();
            }

            UpdateChildren();
        }

        public void UpdateChildren() 
        {
            RandomColor[] list = this.GetComponentsInChildren<RandomColor>();
            foreach (RandomColor c in list)
                c.colorData.Color = colorData.Color;
        }

        public void NewColor()
        {
            colorData.Color = RandomColorPicker.ColorToHex(
                       RandomColorPicker.RetriveRandomColor(
                           RandomColorPicker.DefaultSeperationChar, ColorPalette));
            Renderer renderer = this.GetComponent<Renderer>();
            if(renderer != null)
                foreach(UnityEngine.Material material in renderer.materials)
                    material.color = RandomColorPicker.HexToColor(colorData.Color);
        }

        public void PostDeserialization()
        {
            UpdateChildren();
        }

        public void PreDeserialization()
        {
            return;
        }

        public void PreSerialization()
        {
            return;
        }
    }
}