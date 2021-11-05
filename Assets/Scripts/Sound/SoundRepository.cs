using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Importing;

namespace Sound
{
    public class SoundRepository
    {
        private Database baked_db;
        private Database live_db;
        public SoundRepository() 
        {
            baked_db = Importer.LoadDatabase("Data/Database", "Audio");
            live_db = Importer.Import("Audio", "*.mp3", null, Application.streamingAssetsPath);
        }

        /// <summary>
        /// Returns a list of audio clips from the folder <paramref name="location"/> provided. They should be in "Resources/Audio"
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public List<AudioClip> GrabBakedAudio(string location) 
        {
            location = SanitizePath(location);
            Folder dir = baked_db.Folders[location];
            List<AudioClip> clips = new List<AudioClip>();

            foreach (File file in dir.Files) 
            {
                string clipPath = System.IO.Path.ChangeExtension($"Audio\\{file.FileLocation}", string.Empty);
                clipPath = clipPath.Substring(0, clipPath.Length - 1);
                AudioClip c = Resources.Load<AudioClip>(clipPath);
                if(c != null)
                    clips.Add(c);
            }

            return clips;
        }

        /// <summary>
        /// Returns a list of audio clips from the folder <paramref name="location"/> provided. They should be in "StreamingAssets/Audio"
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public List<AudioClip> GrabLiveAudio(string location)
        {
            location = SanitizePath(location);
            Folder dir = baked_db.Folders[location];
            List<AudioClip> clips = new List<AudioClip>();

            throw new System.NotImplementedException();

            return clips;
        }

        public static string EnviromentSoundBank(EnviromentSound enumm, bool isAnimal = false) 
        {
            string profile = isAnimal ? "Animal" : "Player";
            switch (enumm) 
            {
                default:
                case EnviromentSound.None:
                    return string.Empty;
                case EnviromentSound.Grass:
                    return $"{profile}/Footsteps/Grass";
                case EnviromentSound.Tile:
                    if (isAnimal) return $"{profile}/Footsteps/Wood_Tile";
                    return $"{profile}/Footsteps/Tile";
                case EnviromentSound.Wood:
                    if (isAnimal) return $"{profile}/Footsteps/Wood_Tile";
                    return $"{profile}/Footsteps/Wood";

            }
        }

        private static string SanitizePath(string s)
        {
            return s.Replace('/', '\\');
        }
    }
}
