using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TMPro.TMP_InputField))]
public class TMPInputTextChangeEvent : MonoBehaviour
{
    TMPro.TMP_InputField asset;
    string lastText;

    public UnityEvent OnTextChange;

    void Start()
    {
        asset = GetComponent<TMPro.TMP_InputField>();
        lastText = asset.text;
    }

    private void Update()
    {
        if (!lastText.Equals(asset.text))
        {
            OnTextChange.Invoke();
            lastText = asset.text;
        }
    }


}
