using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RunAtRandom : MonoBehaviour
{
    [SerializeField]
    float initialDelay;
    [SerializeField]
    DataType.Range randomRange;
    [SerializeField]
    UnityEvent OnRun;

    float timeToRun;
    void Start()
    {
        timeToRun = Time.time + randomRange.Read() + initialDelay;
    }

    
    void Update()
    {
        if (Time.time > timeToRun) 
        {
            timeToRun = Time.time + randomRange.Read();
            OnRun.Invoke();
        }
    }
}
