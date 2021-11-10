using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    public class Clock : MonoBehaviour
    {
        TimeController tc;

        [SerializeField]
        Text text;
        void Start()
        {
            tc = Toolbox.Instance.TimeController;
        }

        void Update()
        {
            text.text = tc.ToString();
        }
    }
}