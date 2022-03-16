using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Emotions
{
    public class EmotionCheck : MonoBehaviour, IEmotionCheck
    {
        [SerializeField]
        private string _lastEmotion;
        [SerializeField]
        private string _lastEmotionDetailed;
        public string LastEmotion => _lastEmotion;
        public string LastEmotionDetailed => _lastEmotionDetailed;
        private CreatureController creature;

        public string GrabEmotion() 
        {
            Emotion closest = EmotionCloud.FindClosestMatch(GeneratreEmotion());
            _lastEmotion = closest.BaseEmotion;
            _lastEmotionDetailed = closest.DetailedEmotion;
            return closest.BaseEmotion;
        }

        private int GetSatisfaction() 
        {
            return (int)creature.Satisfaction;
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