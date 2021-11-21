using System.Collections;
using System.Collections.Generic;

namespace DataType
{
    [System.Serializable]
    public struct Range : IValue
    {
        public float MaxValue => Max;
        public float MinValue => Min;

        public float Min;
        public float Max;

        public Range(float min, float max) 
        {
            Min = min;
            Max = max;
        }

        public Range(float max) 
        {
            Min = 0;
            Max = max;
        }

        public float Read() 
        {
            System.Random r = new System.Random();
            float n = (float)r.NextDouble();
            return (n * (Max-Min)) + Min;
        }
    }
}