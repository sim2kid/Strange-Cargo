using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placement
{
    public class Hologram : MonoBehaviour
    {
        private Vector3 _center;
        public Vector3 Center { get => _center + transform.position; private set => _center = value; }
        public Vector3 HalfExtents { get; private set; }

        void Start()
        {
            Vector3 minBound = Vector3.zero;
            Vector3 maxBound = Vector3.zero;

            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
            foreach (MeshFilter filter in meshFilters) 
            {
                Bounds bounds = filter.mesh.bounds;
                Vector3 localCenter = bounds.center + filter.transform.position;
                minBound = MinVector(
                    Multiply(localCenter - bounds.extents, filter.transform.lossyScale),
                    minBound);
                maxBound = MaxVector(
                    Multiply(localCenter + bounds.extents, filter.transform.lossyScale),
                    maxBound);
            }

            Center = new Vector3(
                minBound.x + ((maxBound.x - minBound.x) / 2),
                minBound.y + ((maxBound.y - minBound.y) / 2),
                minBound.z + ((maxBound.x - minBound.z) / 2));
            HalfExtents = maxBound - minBound;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawCube(Center, HalfExtents);


            Gizmos.color = Color.red;
            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
            foreach (MeshFilter filter in meshFilters)
            {
                Bounds bounds = filter.mesh.bounds;
                Vector3 size = Multiply(bounds.size, filter.transform.lossyScale);
                Gizmos.DrawWireCube(bounds.center + filter.transform.position, size);
            }
            Gizmos.DrawWireCube(Center, HalfExtents);
        }

        private Vector3 Multiply(Vector3 A, Vector3 B) 
        {
            return new Vector3(A.x * B.x, A.y * B.y, A.z * B.z);
        }

        private Vector3 MaxVector(params Vector3[] vectors) 
        {
            if(vectors.Length == 0)
                return Vector3.zero;
            Vector3 max = vectors[0];
            foreach (Vector3 v in vectors)
            { 
                max.x = Mathf.Max(max.x, v.x);
                max.y = Mathf.Max(max.y, v.y);
                max.z = Mathf.Max(max.z, v.z);
            }
            return max;
        }

        private Vector3 MinVector(params Vector3[] vectors)
        {
            if (vectors.Length == 0)
                return Vector3.zero;
            Vector3 min = vectors[0];
            foreach (Vector3 v in vectors)
            {
                min.x = Mathf.Min(min.x, v.x);
                min.y = Mathf.Min(min.y, v.y);
                min.z = Mathf.Min(min.z, v.z);
            }
            return min;
        }
    }
}