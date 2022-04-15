using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataType;
using UnityEngine.Events;

public class RandomCall : MonoBehaviour
{
    public Range range;
    public UnityEvent OnCall;

    private float current;

    // Update is called once per frame
    void Update()
    {
        current -= Time.deltaTime;
        if (current < 0)
        {
            current += range.Read();
            OnCall.Invoke();
        }
    }
}
