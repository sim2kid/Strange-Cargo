using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Face
{
    public class FaceRepository : MonoBehaviour
    {
        public List<string> FaceTypes { get; private set; } = new List<string>();
        public FaceRepository() 
        {
            var db = Importing.Importer.Import(System.IO.Path.Combine(Application.persistentDataPath, "Faces"),
                null, "", ".png");
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}