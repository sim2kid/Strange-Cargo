using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Placement
{
    public class GridOrigin : MonoBehaviour
    {
        public Vector3 Origin => this.transform.position;
        public Quaternion Rotation => this.transform.rotation;
        [Range(0.01f, 2f)]
        public float Size = 1f;

        private void OnValidate()
        {
            if (Size == 0) 
            {
                Size = 0.01f;
            }
        }

        void Start()
        {

        }

        public Vector3 GetGridPosition(Vector3 location) 
        {
            // Convert location so Origin is the same
            location -= Origin;
            // Reduce and truncate the grid
            location = new Vector3 (Mathf.Round(location.x / Size), Mathf.Round(location.y / Size), Mathf.Round(location.z / Size));
            return location;
        }

        public Vector3 ToWorldPosition(Vector3 location) 
        {
            location *= Size; // Resize to global grid
            location += Origin; // Set grid back to origin
            return location;
        }

        public Quaternion SetRotation(Quaternion roataion, float offset = 0) 
        {
            var eular = Rotation.eulerAngles;
            roataion = Quaternion.Euler(eular.x, offset, eular.z);
            return roataion;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            var sceneCam = SceneView.lastActiveSceneView;
            Vector3 position = sceneCam.pivot;
            //if (Mathf.Abs(position.y - Origin.y) > 100)
            //    return;
            Vector3 UseOrigin = GetGridPosition(position);
            UseOrigin = Origin + (UseOrigin * Size);
            UseOrigin.y = Origin.y;
            Gizmos.color = Color.cyan;
            int drawNumLines = 50;
            for (int i = -drawNumLines; i < drawNumLines; i++) 
            {
                Gizmos.DrawLine(UseOrigin + new Vector3(i * Size, 0, -drawNumLines * Size), UseOrigin + new Vector3(i * Size, 0, drawNumLines * Size));
                Gizmos.DrawLine(UseOrigin + new Vector3(-drawNumLines * Size, 0, i * Size), UseOrigin + new Vector3(drawNumLines * Size, 0, i * Size));
            }
        }
#endif
    }
}