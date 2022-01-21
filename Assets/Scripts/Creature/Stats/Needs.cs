using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

namespace Creature.Stats
{
    [Serializable]
    public struct Needs
    {
        [JsonIgnore]
        public float Min => 0;
        [JsonIgnore]
        public float Max => 200;

        [SerializeField]
        float[] _myNeeds;

        public float Appetite { get => this[0]; set => this[0] = value; }
        public float Bladder { get => this[1]; set => this[1] = value; }
        public float Social { get => this[2]; set => this[2] = value; }
        public float Energy { get => this[3]; set => this[3] = value; }
        public float Hygiene { get => this[4]; set => this[4] = value; }
        public float Happiness { get => this[5]; set => this[5] = value; }

        public float this[int index]
        {
            get
            {
                if (_myNeeds == null)
                    initilize(this.Max);
                if (_myNeeds.Length == 0)
                    initilize();
                return _myNeeds[index];
            }
            set
            {
                if (_myNeeds == null)
                    initilize(this.Max);
                if (_myNeeds.Length == 0)
                    initilize();
                _myNeeds[index] = value;
            }
        }
        [JsonIgnore]
        public int Count 
        {  
            get 
            {
                if(_myNeeds == null) 
                    initilize(this.Max);
                else if (_myNeeds.Length == 0)
                    initilize();
                return _myNeeds.Length;
            } 
        }

        public void Clamp() 
        {
            Clamp(Min, Max);
        }

        public void Clamp(float min, float max)
        {
            for(int i = 0; i < this.Count; i++)
                this[i] = Mathf.Clamp(this[i], min, max);
        }

        private void initilize(float defaultValue) 
        {
            initilize();
            for (int i = 0; i < _myNeeds.Length; i++)
                _myNeeds[i] = defaultValue;
        }

        private void initilize()
        {
            _myNeeds = new float[6];
        }

        public static Needs Zero => new Needs(0);

        public Needs(float baseAbount) 
        {
            _myNeeds = new float[0];
            initilize(baseAbount);
        }
        public Needs(float[] newNeeds) 
        {
            _myNeeds = new float[0];
            initilize();
            for (int i = 0; i < _myNeeds.Length && i < newNeeds.Length; i++)
                _myNeeds[i] = newNeeds[i];
        }
        public Needs(float appetite, float bladder, float social, float energy, float hygiene, float happiness) 
        {
            _myNeeds = new float[0];
            float[] f = new float[] {
                appetite,
                bladder,
                social,
                energy,
                hygiene,
                happiness
            };
            _myNeeds = f;
        }

        public Needs Clone()
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

        public static explicit operator Needs(float[] other) 
        {
            return new Needs(other);
        }

        public static implicit operator float[](Needs other)
        {
            return other._myNeeds;
        }

        public override string ToString()
        {
            string s = string.Empty;
            foreach (float f in _myNeeds)
                s += $" {f},";
            return $"Range: [ {this.Min}, {this.Max} ], Value: [{s.Substring(0, s.Length - 2)} ]";
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