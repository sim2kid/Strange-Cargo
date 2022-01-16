using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class EditorOnly : MonoBehaviour
    {
        void Start()
        {
            if (Application.isEditor)
                Destroy(this);
            else
                Destroy(this.gameObject);
        }
    }
}