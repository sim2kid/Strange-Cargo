using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genetics
{
    [Serializable]
    public class BodyPart
    {
        public string Hash;
        public string Type;
        public string Name;
        public string FileLocation;
        public string Sound;
        public string OffsetBone;
        public Vector3 Offset;
        public float Scale;
        public ShaderEnum Shader;
        public List<string> Patterns;
        public string Eyes;
        public string Mouth;
    }
}