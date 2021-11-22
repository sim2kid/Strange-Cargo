using System.Collections;
using System.Collections.Generic;

namespace DataType
{
    [System.Serializable]
    public struct ValueRange : IValue
    {
        public float MaxValue => Value + Range.MaxValue;
        public float MinValue => Value - Range.MinValue;

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

        public float Read() 
        {
            return Value + Range.Read();
        }



        public static explicit operator float(ValueRange x) 
        {
            return x.Read();
        }
        public static implicit operator ValueRange(float x)
        {
            return new ValueRange(x);
        }
        public static implicit operator ValueRange(Range x)
        {
            return new ValueRange(0, x);
        }

        public static ValueRange operator *(ValueRange x, float y)
        {
            return new ValueRange(x.Read() * y);
        }
        public static ValueRange operator *(float x, ValueRange y)
        {
            return new ValueRange(x * y.Read());
        }

        public static ValueRange operator +(ValueRange x, float y)
        {
            return new ValueRange(x.Read() + y);
        }
        public static ValueRange operator +(float x, ValueRange y)
        {
            return new ValueRange(x + y.Read());
        }

        public static ValueRange operator -(ValueRange x, float y)
        {
            return new ValueRange(x.Read() - y);
        }
        public static ValueRange operator -(float x, ValueRange y)
        {
            return new ValueRange(x - y.Read());
        }

        public static ValueRange operator /(ValueRange x, float y)
        {
            return new ValueRange(x.Read() / y);
        }
        public static ValueRange operator /(float x, ValueRange y)
        {
            return new ValueRange(x / y.Read());
        }

        public static ValueRange operator %(ValueRange x, float y)
        {
            return new ValueRange(x.Read() % y);
        }
        public static ValueRange operator %(float x, ValueRange y)
        {
            return new ValueRange(x % y.Read());
        }

        public static bool operator ==(ValueRange x, ValueRange y)
        {
            return x.Equals(y);
        }
        public static bool operator !=(ValueRange x, ValueRange y)
        {
            return !x.Equals(y);
        }

        public override bool Equals(object other)
        {
            if (typeof(object) == typeof(ValueRange))
                return this.MaxValue == ((ValueRange)other).MaxValue &&
                    this.MinValue == ((ValueRange)other).MinValue &&
                    this.Value == ((ValueRange)other).Value;
            if (typeof(object) == typeof(float))
                return this.MaxValue == 0 && this.MaxValue == 0 && this.Value == (float)other;

            return false;
        }
        public override int GetHashCode()
        {
            return System.Tuple.Create(MaxValue,MinValue,Value).GetHashCode();
        }
    }
}