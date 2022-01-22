using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sound.Player;

[RequireComponent(typeof(AudioPlayer))]
public class Footsteps : MonoBehaviour
{
    public float StepRate = 1.4f;
    public string StepRepository;
    public float CheckDistance;

    private float timer;
    private Vector3 lastPos;
    private AudioPlayer ap;

    void Start()
    {
        ap = GetComponent<AudioPlayer>();
    }


    void Update()
    {
        
    }
}
