using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    public class AreaZone : MonoBehaviour
    {
        List<Area> allAreas;

        void Start()
        {
            allAreas = new List<Area>();
            foreach (var collider in GetComponentsInChildren<BoxCollider>()) 
            {
                allAreas.Add(new Area(collider));
                collider.enabled = false;
                var renderer = collider.gameObject.GetComponent<MeshRenderer>();
                if (renderer != null)
                { 
                    renderer.enabled = false;
                }
            }
        }

        public float GetTotalVolume() 
        {
            float volume = 0;
            foreach (var area in allAreas)
            {
                volume += area.Volume;
            }
            return volume;
        }

        public Vector3 GetRandomPoint() 
        {
            float randomPoint = GetTotalVolume() * Random.value;
            float runningVolume = 0;
            foreach (var area in allAreas) 
            {
                runningVolume += area.Volume;
                if (randomPoint <= runningVolume) 
                {
                    return area.RandomAreaInBounds();
                }
            }
            return Vector3.zero;
        }
    }
}