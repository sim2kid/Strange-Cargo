using System.Collections;
using System.Collections.Generic;

namespace DataType
{
    [System.Serializable]
    public class ValueRange
    {
        public float Value;
        public Range Range;

        public ValueRange(float value, Range range)
        {
            Value = value;
            Range = range;
        }

        public ValueRange(float value, float min, float max) 
        {
            Value = value;
            Range = new Range(min, max);
        }

        public ValueRange(float value, float max)
        {
            Value = value;
            Range = new Range(max);
        }

        public ValueRange(float value)
        {
            Value = value;
            Range = new Range();
        }

        public ValueRange() 
        {
            Value = 0;
            Range = new Range();
        }

        public float Read() 
        {
            return Value + Range.Read();
        }
    }
}