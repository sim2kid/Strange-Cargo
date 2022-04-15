using Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MarkPoop : MonoBehaviour
{
    public UnityEvent OnEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Poop"))
        {
            GameObject poop = other.gameObject;
            Destroy(poop.GetComponent<Pickupable>());
            poop.AddComponent<DestoryAfterTime>().secondsUntilDestory = 10;
            Player.Money.Instance.Value += 1;
            OnEnter.Invoke();
        }
    }
}
