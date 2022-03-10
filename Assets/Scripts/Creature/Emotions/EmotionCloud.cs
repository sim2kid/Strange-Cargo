using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Emotions
{
    public static class EmotionCloud
    {
        private static List<Emotion> _emotions;
        public static List<Emotion> Emotions
        {
            get
            {
                if (_emotions == null)
                {
                    SetEmotions();
                }
                return _emotions;
            }
        }

        public static Emotion FindClosestMatch(Emotion e) 
        {
            Emotion closest = null;
            float distance = float.MaxValue;
            foreach (Emotion m in Emotions) 
            {
                float dis = Emotion.Distance(m, e);
                if (dis < distance) 
                {
                    distance = dis;
                    closest = m;
                }
            }
            return closest;
        }

        private static void SetEmotions()
        {
            _emotions = new List<Emotion>();
            TextAsset text = Resources.Load<TextAsset>("Data/Emotions");
            foreach (string line in text.text.Split(',')) 
            {
                Emotion e = new Emotion(line);
                _emotions.Add(e);
            }
            Console.Log($"Emotions have been generated. " +
                $"There are a total of {_emotions.Count} emotions registered.");
        }
    }
}