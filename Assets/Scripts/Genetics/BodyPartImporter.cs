using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utility;

namespace Genetics
{
    public static class BodyPartImporter
    {
        private const string BODYPART_FOLDER = "Models";
        public static void Import() 
        {
            GeneticRepository genePool = Toolbox.Instance.GenePool;
            string partPath = Path.Combine(Application.streamingAssetsPath, BODYPART_FOLDER);
            string[] paths = Directory.GetFiles(partPath, "*.gltf");

        }

        static void AddPart(GeneticRepository genePool, string filePath) 
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

            // Add the Bodypart to the partList

        }
    }
}