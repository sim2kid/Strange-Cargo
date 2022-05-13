using Environment.Tub;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoapBubble : MonoBehaviour
{
    [SerializeField]
    float timeLeft;
    float timeAlive = 5f;
    [SerializeField]
    float force = 1;
    Rigidbody body;
    public Bathtub tub;
    public float bubbleWorth = 0.1f;

    public void OnEnable()
    {
        timeLeft = timeAlive;
        if (body == null) 
        {
            body = GetComponent<Rigidbody>();
        }
        body.velocity = Vector3.zero;
        body.AddForce(transform.up * force, ForceMode.Impulse);
    }

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        tub = FindObjectOfType<Bathtub>();
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0) 
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Creature.CreatureController>() != null) 
        {
            // Hit Creature;
            if (tub != null) 
            {
                tub.ModSoap(bubbleWorth);
            }
        }
    }
}
