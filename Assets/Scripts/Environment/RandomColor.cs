using PersistentData.Loading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Environment
{
    [System.Serializable]
    [RequireComponent(typeof(Renderer))]
    public class RandomColor : MonoBehaviour, PersistentData.Saving.ISaveable
    {
        public string Color;
        [Newtonsoft.Json.JsonIgnore]
        public string ColorPalette = "Data/ColorPalette/default";

        public ILoadable Loadable => null;

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
            this.GetComponent<Renderer>().material.color = RandomColorPicker.HexToColor(Color);
        }
    }
}