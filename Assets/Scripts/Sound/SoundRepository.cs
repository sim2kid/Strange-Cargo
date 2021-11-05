using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Importing;

namespace Sound
{
    public class SoundRepository
    {
        private Database _db;
        public SoundRepository(string soundLocation) 
        {
            _db = Importer.Import(soundLocation, "*.mp3");
        }

        public List<AudioClip> GrabAudio(string location) 
        {
            Folder dir = _db.Folders[location];
            List<AudioClip> clips = new List<AudioClip>();

            foreach (File file in dir.Files) 
            {
                
            }

            return clips;
        }
    }
}
