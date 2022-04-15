using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryAfterTime : MonoBehaviour
{
    public float secondsUntilDestory = float.MaxValue;
    private void Update()
    {
        secondsUntilDestory -= Time.deltaTime;
        if (secondsUntilDestory < 0) 
        {
            Destroy(gameObject);
        }
    }
}
