using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

namespace Genetics
{
    [Serializable]
    public class DNA
    {
        public List<PartHash> BodyPartHashs = new List<PartHash>();
        [JsonIgnore]
        public Color[] Colors
        {
            get
            {
                Color[] colors = new Color[HexColors.Length];
                for (int i = 0; i < HexColors.Length; i++)
                    colors[i] = Utility.RandomColorPicker.HexToColor(HexColors[i]);
                return colors;
            }
            set
            {
                for (int i = 0; i < HexColors.Length; i++)
                {
                    if (i < value.Length)
                        HexColors[i] = Utility.RandomColorPicker.ColorToHex(value[i]);
                    else
                        HexColors[i] = Utility.RandomColorPicker.ColorToHex(Color.magenta);
                }
            }
        }
        public string[] HexColors = new string[3];
        public string FaceType;
    }
}
