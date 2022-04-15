using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    public class Calendar : MonoBehaviour
    {
        DateController dc;

        [SerializeField]
        Text text;
        // Start is called before the first frame update
        void Start()
        {
            dc = Toolbox.Instance.DateController;
        }

        // Update is called once per frame
        void Update()
        {
            text.text = dc.ToString().Replace(' ', '\n');
        }
    }
}
