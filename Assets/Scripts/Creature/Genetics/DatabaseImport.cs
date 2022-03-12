using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Importing;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Genetics
{
    public static class DatabaseImport
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

        public static void Import(GeneticRepository genePool)
        {
            // Find all the patterns
            string texturePath = PathUtil.SanitizePath(Path.Combine(Application.streamingAssetsPath, TEXTURE_FOLDER));
            Database textures = Importing.Importer.Import("", null, texturePath, ".png", ".jpg", ".jpeg");
            foreach (var folder in textures.Folders.Values) 
            {
                foreach (var file in folder.Files) 
                {
                    string hash = GetHashString(PathUtil.SanitizePath(file.FileLocation));
                    Pattern pattern = new Pattern()
                    {
                        Name = file.FileName,
                        FileLocation = file.FileLocation,
                        Hash = hash
                    };
                    genePool.AddPattern(pattern);
                }
            }

            // Find all the body parts
            string partPath = PathUtil.SanitizePath(Path.Combine(Application.streamingAssetsPath, BODYPART_FOLDER));
            Database parts = Importing.Importer.Import("",null, partPath, ".gltf");
            Database partInfo = Importing.Importer.Import("", null, partPath, ".json");
            foreach (var folder in parts.Folders.Values)
            {
                Dictionary<string, BodyPart> partList = genePool.AddPartList(folder.FolderName);
                foreach (var file in folder.Files)
                {
                    string hash = GetHashString(PathUtil.SanitizePath(file.FileLocation));
                    string name = Path.GetFileNameWithoutExtension(file.FileName);
                    string partJsonName = name + ".json";
                    var jsonFile = partInfo.Folders[file.ParentFolder].Files.FirstOrDefault(
                        x => x.FileName.ToLower().Equals(partJsonName.ToLower()));

                    if (jsonFile == null)
                    {
                        Console.Log($"Could not find config file for \"{file.FileLocation}\"");
                    }

                    var jsonTxt = System.IO.File.ReadAllText(PathUtil.SanitizePath(Path.Combine(
                        Application.streamingAssetsPath, BODYPART_FOLDER, jsonFile.FileLocation)));

                    var json = JToken.Parse(jsonTxt);
                    try
                    {
                        // Variable Pre processing

                        var offset = json["Offset"].Values<float>();
                        Vector3 offsetVector = Vector3.zero;
                        if (offset.Count() == 3)
                        {
                            offsetVector = new Vector3(offset.ElementAt(0), offset.ElementAt(1), offset.ElementAt(2));
                        }

                        // patterns!!
                        List<string> patterns = json["Patterns"].Values<string>()?.ToList() ?? new List<string>();

                        string patternList = json["PatternList"].Value<string>() ?? string.Empty;
                        if (!string.IsNullOrEmpty(patternList))
                        {
                            foreach (var patternFile in textures.Folders[patternList].Files)
                            {
                                patterns.Add(patternFile.FileLocation);
                            }
                        }

                        // Create bodypart
                        BodyPart bodyPart = new BodyPart()
                        {
                            Hash = hash,
                            Name = name,
                            Type = file.ParentFolder,
                            FileLocation = file.FileLocation,
                            Sound = json["Sound"].Value<string>() ?? string.Empty,
                            OffsetBone = json["OffsetBone"].Value<string>() ?? string.Empty,
                            Offset = offsetVector,
                            Shader = (ShaderEnum)(json["Shader"].Value<int?>() ?? 0),
                            Patterns = patterns,
                            Mouth = json["Mouth"].Value<string>() ?? string.Empty,
                            Eyes = json["Eyes"].Value<string>() ?? string.Empty,
                        };

                        partList.Add(hash, bodyPart);
                    }
                    catch (System.Exception e) 
                    {
                        Console.LogError($"The body part {file.FileName} in {file.ParentFolder} could not be configured and will not be loaded. Check the Json of the same name as the part. There may be errors.\n{e}");
                    }
                }
            }
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