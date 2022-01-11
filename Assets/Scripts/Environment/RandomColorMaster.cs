using PersistentData.Component;
using PersistentData.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;


namespace Environment
{
    public class RandomColorMaster : MonoBehaviour, PersistentData.Component.ISaveable
    {
        public ColorData colorData;
        public string ColorPalette = "Data/ColorPalette/default";

        public ISaveData saveData { get => colorData; set { colorData = (ColorData)value; } }

        ISaveData ISaveable.saveData { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

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
            this.GetComponent<Renderer>().material.color = RandomColorPicker.HexToColor(colorData.Color);
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