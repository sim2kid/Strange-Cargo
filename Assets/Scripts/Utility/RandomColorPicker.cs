using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public static class RandomColorPicker
    {
        /// <summary>
        /// The location of the color palette file inside of the Resources folder
        /// </summary>
        public static string DefaultColorPalette = "Data/hexColors";
        /// <summary>
        /// The default seperation character for the default color palette file
        /// </summary>
        public static char DefaultSeperationChar = '\n';

        /// <summary>
        /// Returns a random Color from the default color palette file
        /// </summary>
        /// <returns></returns>
        public static Color RetriveRandomColor() 
        {
            return RetriveRandomColor(DefaultSeperationChar, DefaultColorPalette);
        }

        /// <summary>
        /// Returns a random color from the file at <paramref name="resourceFileLocation"/> who's values are seperated by <paramref name="seperationChar"/>.
        /// </summary>
        /// <param name="resourceFileLocation"></param>
        /// <param name="seperationChar"></param>
        /// <returns></returns>
        public static Color RetriveRandomColor(char seperationChar, params string[] resourceFileLocations)
        {
            List<string> Hex = new List<string>();
            foreach (string fileLocation in resourceFileLocations)
            {
                string[] colorHex = grabColorsFromFile(fileLocation, seperationChar);
                Hex.AddRange(colorHex);
            }

            string returnHex = Hex[Random.Range(0, Hex.Count)].Trim();
            return hexToColor(returnHex);
        }

        private static Color hexToColor(string hex) 
        {
            string r = hex.Substring(0, 2);
            string g = hex.Substring(2, 2);
            string b = hex.Substring(4, 2);
            try
            {
                int red = int.Parse(r, System.Globalization.NumberStyles.HexNumber);
                int green = int.Parse(g, System.Globalization.NumberStyles.HexNumber);
                int blue = int.Parse(b, System.Globalization.NumberStyles.HexNumber);

                Color returnColor = new Color(red/255f, green/255f, blue/255f);

                return returnColor;
            }
            catch 
            {
                #if UNITY_EDITOR
                Debug.LogWarning("Could not convert hex color to rgb. Make sure your color palette file is formated in hex! This will throw an exception on a build!!");
                return new Color(255, 0, 255);
                #endif
                throw new System.Exception("Hex Could Not Be Converted to RGB. Make sure the Color Palette File is formatted correctly.");
            }

        }

        private static string[] grabColorsFromFile(string resourceFileLocation, char seperationChar) 
        {
            TextAsset fileData = (TextAsset)Resources.Load(resourceFileLocation);
            string fileString = fileData.text;
            return fileString.Split(seperationChar);
        }
    }
}