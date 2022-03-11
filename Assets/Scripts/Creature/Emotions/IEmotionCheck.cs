using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Emotions
{
    public interface IEmotionCheck
    {
        public string GrabEmotion();
        public string LastEmotion { get; }
        public string LastEmotionDetailed { get; }
    }
}
