using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacialExpressionTriggerDemo : MonoBehaviour
{
    private FacialExpressionDemo facialExpressionScript;
    private GameObject player;
    private void Start()
    {
        GetPlayer();
        GetFacialExpressionScript();
    }
    private void GetPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void GetFacialExpressionScript()
    {
        facialExpressionScript = player.GetComponentInChildren<FacialExpressionDemo>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            facialExpressionScript.SetFacialExpressionTo(1);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            facialExpressionScript.SetFacialExpressionToDefault();
        }
    }
}
