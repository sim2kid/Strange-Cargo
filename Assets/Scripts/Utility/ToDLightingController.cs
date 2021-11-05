using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class ToDLightingController : MonoBehaviour
{
    private TimeController time;

    [Header("Sunlight Direction Offset")]

    [SerializeField]
    [Tooltip("Rotates the sun so it can rise/set in different directions.")]
    private float rotationDirection = -90;
    
    [SerializeField]
    [Tooltip("Modifies the starting position of the sun. The 0th/24th hour is at this location.")]
    private float sunStartingPosition = -90;

    // Start is called before the first frame update
    void Start()
    {
        time = Toolbox.Instance.TimeController;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3((time.DayProgress * 360) + sunStartingPosition, rotationDirection, 0));
    }
}
