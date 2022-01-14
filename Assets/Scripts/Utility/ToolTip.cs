using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Utility
{
    
    public class ToolTip : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI Hover;
        [SerializeField]
        private TextMeshProUGUI Use;
        [SerializeField]
        private TextMeshProUGUI Hold;

        [HideInInspector]
        public string HoverText;
        [HideInInspector]
        public string UseText;
        [HideInInspector]
        public string HoldText;

        private void Awake()
        {
            Toolbox.Instance.ToolTip = this;
        }

        private void Start()
        {
            Clear();
        }

        public void Clear() 
        {
            HoverText = string.Empty;
            UseText = string.Empty;
            HoldText = string.Empty;
        }

        void Update()
        {
            if(Hover != null)
                Hover.text = ToolTipIcons.ResolveString(HoverText);
            if (Use != null)
                Use.text = ToolTipIcons.ResolveString(UseText);
            if (Hold != null)
                Hold.text = ToolTipIcons.ResolveString(HoldText);
        }
    }
}