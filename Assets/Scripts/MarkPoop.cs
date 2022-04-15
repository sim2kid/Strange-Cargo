using Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkPoop : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Poop")) 
        {
            GameObject poop = collision.gameObject;
            Destroy(poop.GetComponent<Pickupable>());
            poop.AddComponent<DestoryAfterTime>().secondsUntilDestory = 10;
        }
    }
}
