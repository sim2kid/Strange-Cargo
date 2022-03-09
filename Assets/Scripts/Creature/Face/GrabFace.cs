using Genetics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Creature.Face
{
    public class GrabFace : IGrabFace
    {
        public List<string> FaceTypes => faces.FaceTypes;


        string FaceType;
        FaceRepository faces;

        public GrabFace()
        {
            FaceType = "Generic";
            faces = Toolbox.Instance.FaceRepo;
        }

        public Texture2D GrabEyes(string action)
        {
            return GetTex("Eyes", action);
        }

        public Texture2D GrabMouth(string action)
        {
            return GetTex("Mouth", action);
        }

        private Texture2D GetTex(string cat, string action) 
        {
            var tex = faces.GetTexture(FaceType,
                    PathUtil.SanitizePath(System.IO.Path.Combine(cat, action + ".png")));
            if (tex == null)
            {
                tex = Resources.Load<Texture2D>("Face/Empty");
            }
            return tex;
        }

        public void Hydrate(DNA creatureDna)
        {
            if (!string.IsNullOrEmpty(creatureDna.FaceType))
            {
                this.FaceType = creatureDna.FaceType;
            }
            else 
            {
                Console.LogWarning($"Creature with no face type has been detected.");
            }
        }
    }
}