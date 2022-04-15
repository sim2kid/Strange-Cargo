using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class DisplayMoney : MonoBehaviour
    {
        TMPro.TMP_Text tmp;
        private string originalText;
        private void Start()
        {
            tmp = GetComponent<TMPro.TMP_Text>();
            originalText = tmp.text;
        }
        private void Update()
        {
            if (tmp != null) 
            {
                tmp.text = Replace(originalText);
            }
        }

        public static string Replace(string input) 
        {
            var moneyValue = Player.Money.Instance.Value;
            return input.Replace("{money}", moneyValue.ToString("0.00"));
        }
    }
}