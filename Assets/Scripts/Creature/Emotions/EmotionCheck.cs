using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Emotions
{
    public class EmotionCheck : MonoBehaviour, IEmotionCheck
    {
        

        private CreatureController creature;

        public string GrabEmotion() 
        {
            Emotion closest = EmotionCloud.FindClosestMatch(GeneratreEmotion());
            return closest.BaseEmotion;
        }

        private int GetSatisfaction() 
        {
            return 50;
        }
        private int GetBeauty()
        {
            return 50;
        }
        private Emotion GeneratreEmotion() 
        {
            return new Emotion()
            {
                BaseEmotion = "Unknown",
                DetailedEmotion = "Unknown",
                Appetite = (int)creature.needs.Appetite,
                Bladder = (int)creature.needs.Bladder,
                Social = (int)creature.needs.Social,
                Energy = (int)creature.needs.Energy,
                Hygiene = (int)creature.needs.Hygiene,
                Satisfaction = GetSatisfaction(),
                EnviromentalBeauty = GetBeauty()
            };
        }


        void Start() 
        {
            creature = GetComponent<CreatureController>();
            if (creature == null) 
            {
                Console.LogError($"EmotionCheck can only be added to a creature. \"{name}\" had no CreatureController.");
                Destroy(this);
            }
        }
    }
}