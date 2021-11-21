using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Creature.Stats
{
    [Serializable]
    public struct Needs : ICloneable
    {
        public float Min => 0;
        public float Max => 200;

        [SerializeField]
        float[] _myNeeds;

        public float Appetite { get => _myNeeds[0]; set => _myNeeds[0] = value; }
        public float Bladder { get => _myNeeds[1]; set => _myNeeds[1] = value; }
        public float Social { get => _myNeeds[2]; set => _myNeeds[2] = value; }
        public float Energy { get => _myNeeds[3]; set => _myNeeds[3] = value; }
        public float Hygiene { get => _myNeeds[4]; set => _myNeeds[4] = value; }
        public float Happiness { get => _myNeeds[5]; set => _myNeeds[5] = value; }

        public float this[int index]
        {
            get
            {
                return _myNeeds[index];
            }
            set
            {
                _myNeeds[index] = value;
            }
        }
        public int Count => _myNeeds.Length;

        public Needs(float baseAbount) 
        {
            _myNeeds = new float[6];
            for(int i = 0; i < _myNeeds.Length; i++)
                _myNeeds[i] = baseAbount;
        }
        public Needs(float[] newNeeds) 
        {
            _myNeeds = new float[6];
            for (int i = 0; i < _myNeeds.Length && i < newNeeds.Length; i++)
                _myNeeds[i] = newNeeds[i];
        }
        public Needs(float appetite, float bladder, float social, float energy, float hygiene, float happiness) 
        {
            _myNeeds = new float[] {
                appetite,
                bladder,
                social,
                energy,
                hygiene,
                happiness
            };
        }

        public object Clone()
        {
            return new Needs((float[])_myNeeds.Clone());
        }

        public static Needs operator +(Needs x, Needs y) 
        {
            float[] r = new float[Mathf.Min(x.Count, y.Count)];
            for (int i = 0; i < r.Length; i++)
                r[i] = x[i] + y[i];
            return new Needs(r);
        }
        public static Needs operator +(Needs x, float[] y)
        {
            return x + new Needs(y);
        }
        public static Needs operator +(Needs x, float y)
        {
            return x + new Needs(y);
        }
        public static Needs operator ++(Needs x)
        {
            return x + 1;
        }

        public static Needs operator -(Needs x, Needs y)
        {
            float[] r = new float[Mathf.Min(x.Count, y.Count)];
            for (int i = 0; i < r.Length; i++)
                r[i] = x[i] - y[i];
            return new Needs(r);
        }
        public static Needs operator -(Needs x, float[] y)
        {
            return x - new Needs(y);
        }
        public static Needs operator -(Needs x, float y)
        {
            return x - new Needs(y);
        }
        public static Needs operator -(Needs x)
        {
            return x * -1;
        }
        public static Needs operator --(Needs x)
        {
            return x - 1;
        }


        public static Needs operator *(Needs x, Needs y)
        {
            float[] r = new float[Mathf.Min(x.Count, y.Count)];
            for (int i = 0; i < r.Length; i++)
                r[i] = x[i] * y[i];
            return new Needs(r);
        }
        public static Needs operator *(Needs x, float[] y)
        {
            return x * new Needs(y);
        }
        public static Needs operator *(Needs x, float y)
        {
            return x * new Needs(y);
        }


        public static Needs operator /(Needs x, Needs y)
        {
            float[] r = new float[Mathf.Min(x.Count, y.Count)];
            for (int i = 0; i < r.Length; i++)
                r[i] = x[i] / y[i];
            return new Needs(r);
        }
        public static Needs operator /(Needs x, float[] y)
        {
            return x / new Needs(y);
        }
        public static Needs operator /(Needs x, float y)
        {
            return x / new Needs(y);
        }


        public static Needs operator %(Needs x, Needs y)
        {
            float[] r = new float[Mathf.Min(x.Count, y.Count)];
            for (int i = 0; i < r.Length; i++)
                r[i] = x[i] % y[i];
            return new Needs(r);
        }
        public static Needs operator %(Needs x, float[] y)
        {
            return x % new Needs(y);
        }
        public static Needs operator %(Needs x, float y)
        {
            return x % new Needs(y);
        }


        public static bool operator ==(Needs x, Needs y)
        {
            bool matching = true;
            for (int i = 0; i < Mathf.Min(x.Count, y.Count); i++)
                if (x[i] != y[i])
                    matching = false;
            return matching;
        }
        public static bool operator ==(Needs x, float[] y)
        {
            return x == new Needs(y);
        }

        public static bool operator !=(Needs x, Needs y)
        {
            bool matching = true;
            for (int i = 0; i < Mathf.Min(x.Count, y.Count); i++)
                if (x[i] != y[i])
                    matching = false;
            return !matching;
        }
        public static bool operator !=(Needs x, float[] y)
        {
            return x != new Needs(y);
        }

        public override bool Equals(object other) 
        {
            if(typeof(object) == typeof(Needs))
                return this == (Needs)other;
            if(typeof(object) == typeof(float[]))
                return this == (float[])other;

            return false;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(_myNeeds).GetHashCode();
        }
    }
}