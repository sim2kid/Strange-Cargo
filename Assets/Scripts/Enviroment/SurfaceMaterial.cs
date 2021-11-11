using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enviroment
{
    public class SurfaceMaterial : MonoBehaviour, IMaterial

    {
        [SerializeField]
        private Material _material = Material.Wood;
        public Material Material => _material;

    }
}