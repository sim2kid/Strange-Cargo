using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Genetics
{
    [Serializable]
    public class DNA
    {
        public List<PartHash> BodyPartHashs = new List<PartHash>();
        public Color[] Colors = new Color[3];
    }
}
