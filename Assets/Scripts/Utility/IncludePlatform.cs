using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncludePlatform : MonoBehaviour
{
    [SerializeField]
    private List<RuntimePlatform> platforms = new List<RuntimePlatform>();
    void Start()
    {
        foreach (RuntimePlatform platform in platforms)
        {
            if (Application.platform == platform)
            {
                Destroy(this);
                return;
            }
        }
        Destroy(this.gameObject);
    }
}
