using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryAfterTime : MonoBehaviour
{
    private float _time = float.MaxValue;
    private float _max = float.MaxValue;
    public float secondsUntilDestory 
    { 
        get => _time; 
        set {
            _time = value;
            _max = value;
        } 
    }


    private void Update()
    {
        _time -= Time.deltaTime;
        if (_time < 0) 
        {
            Destroy(gameObject);
        }
        if (_time > 0) 
        {
            gameObject.transform.localScale = Vector3.one * (_time/_max);
        }
    }
}
