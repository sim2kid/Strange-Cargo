using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class SpawnPrefab : MonoBehaviour
    {
        [SerializeField]
        public GameObject prefab;
        [SerializeField]
        public Vector3 offset;

        public void Spawn() 
        {
            Instantiate(prefab, offset + transform.position, new Quaternion());
        }
    }
}