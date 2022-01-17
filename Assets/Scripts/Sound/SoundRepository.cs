using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Importing;
using UnityEngine.Networking;
using UnityEngine.Events;
using System;

namespace Sound
{
    public class SoundRepository
    {
        private Database baked_db;
        private Database live_db;
        public SoundRepository() 
        {
            baked_db = Importer.LoadDatabase("Data/Database", "Audio");
            live_db = Importer.Import("", null, System.IO.Path.Combine(Application.streamingAssetsPath, "Audio"), ".mp3", ".wav", ".ogg", ".aiff", ".aif");
        }

        /// <summary>
        /// Returns a list of audio clips from the folder <paramref name="location"/> provided. They should be in "Resources/Audio"
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public List<AudioClip> GrabBakedAudio(string location) 
        {
            List<AudioClip> clips = new List<AudioClip>();
            try
            {
                location = SanitizePath(location);
                Folder dir = baked_db.Folders[location];

                foreach (File file in dir.Files)
                {
                    string clipPath = System.IO.Path.ChangeExtension($"Audio\\{file.FileLocation}", string.Empty);
                    clipPath = clipPath.Substring(0, clipPath.Length - 1);
                    AudioClip c = Resources.Load<AudioClip>(clipPath);
                    if (c != null)
                        clips.Add(c);
                }

                return clips;
            }
            catch 
            {
                return clips;
            }
        }

        /// <summary>
        /// Returns an audio clip from the folder <paramref name="location"/> provided. They should be in "Resources/Audio"
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public AudioClip GrabSingleBakedAudio(string location)
        {
            try
            {
                location = SanitizePath(location);
                var path = location.Split('\\');
                location = string.Empty;
                for (int i = 0; i < path.Length - 1; i++)
                {
                    location += path[i] + "\\";
                }
                location = location.Substring(0, location.Length - 1);
                string filename = path[path.Length - 1];

                Folder dir = baked_db.Folders[location];

                var file = dir.Files.Find(x => x.FileName == filename);

                string clipPath = System.IO.Path.ChangeExtension($"Audio\\{file.FileLocation}", string.Empty);
                clipPath = clipPath.Substring(0, clipPath.Length - 1);
                AudioClip clip = Resources.Load<AudioClip>(clipPath);
                return clip;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a list of audio clips from the folder <paramref name="location"/> provided. They should be in "StreamingAssets/Audio".
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public IEnumerator GrabLiveAudio(string location, Action<List<AudioClip>> callback) 
        {
            List<AudioClip> audioList = new List<AudioClip>();
            if (!live_db.Folders.ContainsKey(location))
            {
                callback(audioList);
                yield return null;
            }
            else
            {
                location = SanitizePath(location);
                Folder dir = live_db.Folders[location];
                foreach (File file in dir.Files)
                {
                    string fileLocation = System.IO.Path.Combine(System.IO.Path.Combine("file:///", Application.streamingAssetsPath), "Audio");
                    fileLocation = SanitizePath(System.IO.Path.Combine(fileLocation, file.FileLocation));
                    UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(fileLocation, AudioType.UNKNOWN);
                    yield return request.SendWebRequest();
                    if (request.result == UnityWebRequest.Result.ConnectionError)
                    {
                        Console.LogError($"Failed to find audio at {fileLocation}.\n" + request.error);
                    }
                    else if (request.result == UnityWebRequest.Result.ProtocolError)
                    {
                        Console.LogError($"Failed to find audio at {fileLocation}.\n" + request.error);
                    }
                    else if (request.result == UnityWebRequest.Result.DataProcessingError)
                    {
                        Console.LogError($"Failed to process audio at {fileLocation}.\n" + request.error);
                    }
                    else
                    {
                        AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
                        audioList.Add(clip);
                    }
                }
                callback(audioList);
            }
        }

        /// <summary>
        /// Returns a list of audio clips from the folder <paramref name="location"/> provided. They should be in "StreamingAssets/Audio".
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public IEnumerator GrabSingleLiveAudio(string location, Action<AudioClip> callback)
        {
            location = SanitizePath(location);
            var path = location.Split('\\');
            string folder = string.Empty;
            for (int i = 0; i < path.Length - 1; i++)
            {
                folder += path[i] + "\\";
            }
            folder = folder.Substring(0, folder.Length - 1);
            string filename = path[path.Length - 1];

            if (!live_db.Folders.ContainsKey(folder))
            {
                callback(null);
                yield return null;
            }
            else
            {
                Folder dir = live_db.Folders[folder];

                File file = dir.Files.Find(x => x.FileName == filename);
                string fileLocation = System.IO.Path.Combine(System.IO.Path.Combine("file:///", Application.streamingAssetsPath), "Audio");
                fileLocation = SanitizePath(System.IO.Path.Combine(fileLocation, file.FileLocation));
                UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(fileLocation, AudioType.UNKNOWN);
                yield return request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    Console.LogError($"Failed to find audio at {fileLocation}.\n" + request.error);
                }
                else if (request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Console.LogError($"Failed to find audio at {fileLocation}.\n" + request.error);
                }
                else if (request.result == UnityWebRequest.Result.DataProcessingError)
                {
                    Console.LogError($"Failed to process audio at {fileLocation}.\n" + request.error);
                }
                else
                {
                    AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
                    callback(clip);
                }
            }
            callback(null);
        }

        public static string EnviromentSoundBank(Environment.Material enumm, bool isAnimal = false) 
        {
            string profile = isAnimal ? "Animal" : "Player";
            switch (enumm) 
            {
                default:
                case Environment.Material.None:
                    return string.Empty;
                case Environment.Material.Grass:
                    return $"{profile}/Footsteps/Grass";
                case Environment.Material.Tile:
                    if (isAnimal) return $"{profile}/Footsteps/Wood_Tile";
                    return $"{profile}/Footsteps/Tile";
                case Environment.Material.Wood:
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
