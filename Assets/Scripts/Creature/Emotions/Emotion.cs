using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Emotions
{
    public class Emotion
    {
        public string BaseEmotion;
        public string DetailedEmotion;
        public int Appetite;
        public int Bladder;
        public int Social;
        public int Energy;
        public int Hygiene;
        public int Satisfaction;
        public int EnviromentalBeauty;

        public Emotion() { }
        public Emotion(params string[] csv)
        {
            BaseEmotion = csv[0];
            DetailedEmotion = csv[1];
            Appetite = toInt(csv[2]);
            Bladder = toInt(csv[3]);
            Social = toInt(csv[4]);
            Energy = toInt(csv[5]);
            Hygiene = toInt(csv[6]);
            Satisfaction = toInt(csv[7]);
            EnviromentalBeauty = toInt(csv[8]);
        }

        public static float Distance(Emotion e1, Emotion e2)
        {
            Emotion dif = e1 - e2;
            dif = dif.Pow(2);
            float dist = Mathf.Sqrt(dif.Sum());
            return dist;
        }

        public static Emotion operator -(Emotion a, Emotion b)
        {
            Emotion c = new Emotion()
            {
                BaseEmotion = "Unknown",
                DetailedEmotion = "Unknown",
                Appetite = a.Appetite - b.Appetite,
                Bladder = a.Bladder - b.Bladder,
                Social = a.Social - b.Social,
                Energy = a.Energy - b.Energy,
                Hygiene = a.Hygiene - b.Hygiene,
                Satisfaction = a.Satisfaction - b.Satisfaction,
                EnviromentalBeauty = a.EnviromentalBeauty - b.EnviromentalBeauty
            };
            return c;
        }

        public Emotion Pow(float amount) 
        {
            return Pow(this, amount);
        }

        public static Emotion Pow(Emotion e, float amount) 
        {
            Emotion c = new Emotion()
            {
                BaseEmotion = "Unknown",
                DetailedEmotion = "Unknown",
                Appetite = (int)Mathf.Pow(e.Appetite, amount),
                Bladder = (int)Mathf.Pow(e.Bladder, amount),
                Social = (int)Mathf.Pow(e.Social, amount),
                Energy = (int)Mathf.Pow(e.Energy, amount),
                Hygiene = (int)Mathf.Pow(e.Hygiene, amount),
                Satisfaction = (int)Mathf.Pow(e.Satisfaction, amount),
                EnviromentalBeauty = (int)Mathf.Pow(e.EnviromentalBeauty, amount)
            };
            return c;
        }

        public int Sum() 
        { 
            return Appetite + Bladder + Social + Energy + Hygiene + Satisfaction + EnviromentalBeauty;
        }

        private int toInt(string s) 
        {
            if (s == null)
            {
                return 0;
            }
            if(int.TryParse(s, out int result)) 
            {
                return result;
            }
            return 0;
        }
    }
}