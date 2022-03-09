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
        public FaceRepository() 
        {
            db = Importing.Importer.Import(System.IO.Path.Combine(Application.persistentDataPath, "Faces"),
                null, "", ".png");

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
            category = category.ToLower();
            texture = texture.ToLower();
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
            if (category.Equals("generic") && !db.Folders.ContainsKey(folder))
            {
                Console.LogWarning($"Could not find face type of \"{category}\". Using generic faces instead.");
                return GetTexture("generic", texture);
            }
            // Get category folder
            Folder dir = db.Folders[folder];

            // Check if there's a texture in that folder that matches
            var file = dir.Files.FirstOrDefault(x => x.FileName == filename);

            if (file == null)
            {
                if (category.Equals("generic"))
                {
                    Console.LogError($"No face was found for file \"{texture}\" in the generic category.");
                    return null;
                }
                else 
                {
                    Console.LogWarning($"No face was found for file \"{texture}\" in category \"{category}\". Using generics as a backup.");
                    return GetTexture("generic", texture);
                }
            }

            // Load file
            Texture2D texture2D = new Texture2D(1, 1);
            byte[] textureBytes = System.IO.File.ReadAllBytes(file.FileLocation);

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