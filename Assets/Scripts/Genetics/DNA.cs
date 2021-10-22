using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Genetics
{
    [Serializable]
    public struct DNA
    {
        public List<PartHash> BodyPartHashs;
        public Color[] Colors;
    }
}
