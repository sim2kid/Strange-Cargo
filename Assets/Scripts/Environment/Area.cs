using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    public class Area
    {
        public float Volume => Bounds.size.x + Bounds.size.y + Bounds.size.z;
        public Bounds Bounds { get; private set; }

        public Area() 
        { }
        public Area(BoxCollider boxCollider) 
        {
            Bounds = boxCollider.bounds;
        }

        public Vector3 RandomAreaInBounds() 
        {
            float x = (Bounds.size.x * Random.value);
            float y = (Bounds.size.y * Random.value);
            float z = (Bounds.size.z * Random.value);

            return  (Bounds.center - Bounds.extents) + new Vector3(x, y, z);
        }
    }
}