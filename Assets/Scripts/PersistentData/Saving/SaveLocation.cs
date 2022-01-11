using PersistentData.Loading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Saving
{
    [System.Serializable]
    public class SaveLocation : MonoBehaviour, ISaveable
    {
        public ILoadable Loadable => null;

        float[] Position;
        float[] Rotation;
        float[] Scale;

        public void PostDeserialization()
        {
            transform.localPosition = new Vector3 (Position[0], Position[1], Position[2]);
            transform.localRotation = new Quaternion(Rotation[0], Rotation[1], Rotation[2], Rotation[3]);
            transform.localScale = new Vector3 (Scale[0], Scale[1], Scale[2]);
        }

        public void PreDeserialization()
        {
            return;
        }

        public void PreSerialization()
        {
            Position = new float[]{ transform.localPosition.x, transform.localPosition.y, transform.localPosition.z };
            Rotation = new float[] { transform.localRotation.x, transform.localRotation.y, transform.localRotation.z, transform.localRotation.w };
            Scale = new float[] { transform.localScale.x, transform.localScale.y, transform.localScale.z };
        }
    }

}