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

            Import(genePool);
        }

        /// <summary>
        /// Populate the GenePool with Bodyparts and Patterns
        /// </summary>
        public static void Import(GeneticRepository genePool)
        {
            // Find all the body parts
            string partPath = SanitizePath(Path.Combine(Application.streamingAssetsPath, BODYPART_FOLDER));
            string[] pathsDirectories = Directory.GetDirectories(partPath);
            foreach (string directory in pathsDirectories)
            {
                string[] paths = Directory.GetFiles(directory, "*.gltf");
                foreach (string part in paths)
                    AddPart(genePool, part);
                paths = Directory.GetFiles(directory, "*.ref");
                foreach (string refFile in paths)
                {
                    string refPath = ResolveRef(refFile);
                    if(refPath.EndsWith(".gltf"))
                        AddPart(genePool, refPath);
                }
            }

            // Find all the patterns
            string texturePath = SanitizePath(Path.Combine(Application.streamingAssetsPath, TEXTURE_FOLDER));
            string[] textPaths = Directory.GetFiles(texturePath);
            foreach (string texture in textPaths)
                AddTexture(genePool, texture);
            string[] paths = Directory.GetFiles(texturePath, "*.ref");
            foreach (string refFile in paths)
            {
                string refPath = ResolveRef(refFile);
                if(!string.IsNullOrWhiteSpace(refPath))
                    AddTexture(genePool, refPath);
            }
        }

        private static void AddTexture(GeneticRepository genePool, string filePath) 
        {
            // Check if extension is for a image file
            string extension = Path.GetExtension(filePath).ToLower();
            Regex r = new Regex(@".png|.jpeg|.jpg", RegexOptions.IgnoreCase);
            Match m = r.Match(extension);
            if (!m.Success)
                return;


            string patternName = Path.GetFileName(filePath);

            Pattern p = new Pattern()
            {
                Hash = GetHashString(patternName),
                Name = patternName,
                FileLocation = filePath
            };

            genePool.AddPattern(p);
        }

        private static string ResolveRef(string refPath) 
        {
            if(!File.Exists(refPath))
                return string.Empty;
            string jsonText = File.ReadAllText(refPath);
            JObject jsonObj = (JObject)JToken.Parse(jsonText);
            if(jsonObj.ContainsKey("Path")) 
                return (string)jsonObj["Path"];
            return string.Empty;
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
            if (!File.Exists(jsonLocation))
            {
                Console.LogWarning($"The asset {parent}/{partName} does not have an associated JSON file and will not be added to the Gene Pool.");
                return;
            }

            // Read the json and populate the BodyPart class
            string jsonText = File.ReadAllText(jsonLocation);
            JObject jsonObj = (JObject)JToken.Parse(jsonText);

            float Scale;
            try {
                Scale = (float)jsonObj["Scale"];
            } 
            catch 
            {
                Scale = 0.1f;
            }
            string Sound = string.Empty;
            if (jsonObj.ContainsKey("Sound")) 
            {
                Sound = (string)jsonObj["Sound"];
            }
            string OffsetBone = string.Empty;
            if (jsonObj.ContainsKey("OffsetBone"))
            {
                OffsetBone = (string)jsonObj["OffsetBone"];
            }
            Vector3 Offset = Vector3.zero;
            if (jsonObj.ContainsKey("Offset"))
            {
                Offset = new Vector3((float)jsonObj["Offset"][0], (float)jsonObj["Offset"][1], (float)jsonObj["Offset"][2]);
            }

            // Add the Bodypart to the partList
            BodyPart part = new BodyPart()
            {
                Hash = GetHashString(partName),
                Type = parent,
                Name = partName,
                FileLocation = filePath,
                Sound = Sound,
                OffsetBone = OffsetBone,
                Offset = Offset,
                Scale = Scale,
                Shader = (ShaderEnum)(int)jsonObj["Shader"],
                Patterns = jsonObj["Patterns"].Select(x => (string)x).ToList()
            };

            partList.Add(part.Hash, part);
        }

        private static string SanitizePath(string s) 
        {
            return s.Replace('/','\\');
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