using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using Utility;

namespace Genetics
{
    public static class Importer
    {
        private const string BODYPART_FOLDER = "Models";
        private const string TEXTURE_FOLDER = "Texture";

        /// <summary>
        /// Populate the GenePool with Bodyparts and Patterns
        /// </summary>
        public static void Import() 
        {
            // Grab the genepool from the Toolbox singleton
            GeneticRepository genePool = Toolbox.Instance.GenePool;

            // Find all the body parts
            string partPath = Path.Combine(Application.streamingAssetsPath, BODYPART_FOLDER);
            string[] paths = Directory.GetFiles(partPath, "*.gltf");
            foreach (string part in paths) 
                AddPart(genePool, part);

            // Find all the patterns
            string texturePath = Path.Combine(Application.streamingAssetsPath, TEXTURE_FOLDER);
            string[] textPaths = Directory.GetFiles(partPath);
            foreach (string texture in textPaths) 
                AddTexture(genePool, texture);
        }

        private static void AddTexture(GeneticRepository genePool, string filePath) 
        {
            // Check if extension is for a image file
            string extension = Path.GetExtension(filePath).ToLower();
            Regex r = new Regex(@".png|.jpeg|.jpg", RegexOptions.IgnoreCase);
            Match m = r.Match(extension);
            if (!m.Success)
                return;


            string patternName = Path.GetFileNameWithoutExtension(filePath);

            Pattern p = new Pattern()
            {
                Hash = GetHashString(patternName),
                Name = patternName,
                FileLocation = filePath
            };

            genePool.AddPattern(p);
        }

        private static void AddPart(GeneticRepository genePool, string filePath) 
        {
            string partName = Path.GetFileNameWithoutExtension(filePath);

            // Get the parent folder of the path
            string parent = Path.GetFileName(Path.GetDirectoryName(filePath));

            // Add parent folder to the genepool
            Dictionary<string, BodyPart> partList = genePool.AddPartList(parent);

            // Get the details json by changing the extension to a json
            // Be sure to check if the json exists and throw an error if not.
            string jsonLocation = Path.ChangeExtension(filePath, ".json");
            if (!Directory.Exists(jsonLocation)) 
            {
                Debug.LogWarning($"The asset {parent}/{partName} does not have an associated JSON file and will not be added to the Gene Pool.");
            }

            // Read the json and populate the BodyPart class
            string jsonText = File.ReadAllText(jsonLocation);
            JObject jsonObj = (JObject)JToken.Parse(jsonText);
            // Add the Bodypart to the partList
            BodyPart part = new BodyPart()
            {
                Hash = GetHashString(partName),
                Name = partName,
                FileLocation = filePath,
                Shader = (ShaderEnum)(int)jsonObj["Shader"],
                Patterns = jsonObj["Patterns"].Select(x => (string)x).ToList()
            };

            partList.Add(part.Hash, part);
        }



        // Copied from this:
        // https://stackoverflow.com/questions/3984138/hash-string-in-c-sharp

        /// <summary>
        /// Creates a HashByte from the <paramref name="inputString"/>
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA1.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        /// <summary>
        /// Creates a hash string from the <paramref name="inputString"/>
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}