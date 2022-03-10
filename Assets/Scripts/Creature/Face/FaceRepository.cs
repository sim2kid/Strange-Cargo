using Importing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Creature.Face
{
    public class FaceRepository 
    {
        public List<string> FaceTypes { get; private set; } = new List<string>();
        private Database db;
        private const string generic = "generic";
        public FaceRepository() 
        {
            db = Importing.Importer.Import("", null, 
                System.IO.Path.Combine(Application.streamingAssetsPath, "Face"), ".png");

            foreach (string folders in db.Folders.Keys) 
            {
                if (!folders.Contains("\\")) 
                {
                    FaceTypes.Add(folders);
                }
            }
        }

        public Texture2D GetTexture(string category, string texture) 
        {
            // Clean input
            if (!texture.EndsWith(".png"))
            {
                texture += ".png";
            }

            // Setup for database
            string location = Utility.PathUtil.SanitizePath(System.IO.Path.Combine(category, texture));
            var path = location.Split('\\');
            string folder = string.Empty;
            for (int i = 0; i < path.Length - 1; i++)
            {
                folder += path[i] + "\\";
            }
            folder = folder.Substring(0, folder.Length - 1);
            string filename = path[path.Length - 1];

            // If folder does not exist, try a different one
            if (!db.Folders.ContainsKey(folder))
            {
                if (category.ToLower().Equals(generic.ToLower()))
                {
                    Console.LogError($"Generic folder for face could not be found. " +
                        $"Category: {category} | Texture: {texture} | Folder: {folder} | File: {filename}");
                    return null;
                }
                else
                {
                    Console.LogWarning($"Could not find face type of \"{category}\". Using generic faces instead.");
                    return GetTexture(generic, texture);
                }
            }
            // Get category folder
            Folder dir = db.Folders[folder];

            // Check if there's a texture in that folder that matches
            var file = dir.Files.FirstOrDefault(x => x.FileName.ToLower().Equals(filename.ToLower()));

            if (file == null)
            {
                if (category.ToLower().Equals(generic.ToLower()))
                {
                    Console.LogError($"No face was found for file \"{texture}\" in the generic category.");
                    return null;
                }
                else 
                {
                    Console.LogWarning($"No face was found for file \"{texture}\" in category \"{category}\". Using generics as a backup.");
                    return GetTexture(generic, texture);
                }
            }

            // Load file
            Texture2D texture2D = new Texture2D(1, 1);
            byte[] textureBytes = System.IO.File.ReadAllBytes(System.IO.Path.Combine(Application.streamingAssetsPath, "Face", file.FileLocation));

            if (ImageConversion.LoadImage(texture2D, textureBytes, false)) 
            {
                texture2D.alphaIsTransparency = true;
                return texture2D;
            }

            Console.LogError($"Texture for face at path \"{file.FileLocation}\" could not be loaded.");
            return null;
        }
    }
}