using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcludePlatform : MonoBehaviour
{
    [SerializeField]
    private List<RuntimePlatform> platforms = new List<RuntimePlatform>();
    void Start()
    {
        foreach (RuntimePlatform platform in platforms) 
        {
            if (Application.platform == platform)
            {
                Destroy(this.gameObject);
            }
        }
        Destroy(this);
    }
}
