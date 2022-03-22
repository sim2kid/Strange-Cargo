using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Face
{
    public class AnimationBuilder
    {
        public static FaceAnimation LoadFromResources(string resourceLocation, int priority = 0)
        {
            TextAsset text = Resources.Load<TextAsset>(resourceLocation);
            if (text == null)
            {
                return new FaceAnimation(null, priority);
            }
            List<FaceClip> clips = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FaceClip>>(text.text);
            return new FaceAnimation(clips, priority);
        }
    }
}