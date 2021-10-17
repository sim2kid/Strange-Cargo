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
            return RetriveRandomColor(DefaultColorPalette, DefaultSeperationChar);
        }

        /// <summary>
        /// Returns a random color from the file at <paramref name="resourceFileLocation"/> who's values are seperated by <paramref name="seperationChar"/>.
        /// </summary>
        /// <param name="resourceFileLocation"></param>
        /// <param name="seperationChar"></param>
        /// <returns></returns>
        public static Color RetriveRandomColor(string resourceFileLocation, char seperationChar)
        {
            string[] colorHex = grabColorsFromFile(resourceFileLocation, seperationChar);
            string returnHex = colorHex[Random.Range(0, colorHex.Length)];
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

                return new Color(red, green, blue);
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