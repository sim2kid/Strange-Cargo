using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    /// <summary>
    /// Seconds Elapsed
    /// </summary>
    private float _time;

    [SerializeField]
    private float CurrentTime;

    [Tooltip("The real life minutes per Day/Night cycle")]
    public float MinutesInADay = 13.25f;

    private void OnEnable()
    {
        Utility.Toolbox.Instance.TimeController = this;
    }

    void Start()
    {
        _time = 0;    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Gives seconds elapsed
        _time += Time.fixedDeltaTime;

        if (_time > MinutesInADay * 60) 
        {
            _time -= MinutesInADay * 60;
        }

        Debug.Log(GetTime());

        CurrentTime = GetTime();
    }

    /// <summary>
    /// Returns the time between 0-24 representing the hours of a day
    /// </summary>
    /// <returns></returns>
    public float GetTime() 
    {
        return (_time/(MinutesInADay*60)) * 24;
    }
}
