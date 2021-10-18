using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genetics
{
    public struct BodyPart
    {
        public string Hash;
        public string Name;
        public string FileLocation;
        public ShaderEnum Shader;
        public List<Pattern> Patterns;
    }
}