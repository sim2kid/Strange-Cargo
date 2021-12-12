using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Utility.Input;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ToolTipIcons : MonoBehaviour
{
    TextMeshProUGUI text;
    InputContext context;

    void Start()
    {
        context = FindObjectOfType<InputContext>();
        if (context == null)
        {
            Console.LogError("Missing InputContext component in scene. Will delete ToolTipIcon script to prevent errors");
            Destroy(this);
        }
    }


    void Update()
    {
    }
}
