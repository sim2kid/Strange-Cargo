using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using System.Text.RegularExpressions;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TMPVariables : MonoBehaviour
{
    TextMeshProUGUI tmp;

    string originalText;
    string oldText;

    /// <summary>
    /// populate this field with key value pairs to replace text in a TMP object
    /// </summary>
    public Dictionary<string, string> Variables = new Dictionary<string, string>();

    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();

        originalText = oldText = tmp.text;
    }

    /// <summary>
    /// manualy apply the string resolver
    /// </summary>
    /// <param name="str"></param>
    /// <param name="variables"></param>
    /// <returns></returns>
    public static string ResolveString(string str, Dictionary<string, string> variables) 
    {
        string newString = str;
        if(string.IsNullOrEmpty(newString))
            return str;

        // Find variables listed 
        MatchCollection matches = Regex.Matches(newString, @"(?<=\{).*?(?=\})");
        List<string> matchList = matches.Cast<Match>().Select(match => match.Value).ToList();

        foreach (string vari in matchList) 
        { 
            string originalName = $"{{{vari}}}";

            if (variables.TryGetValue(vari, out string value)) 
            {
                newString.Replace(originalName, value);
            }
        }

        return newString;
    }


    void Update()
    {
        if (tmp.text != oldText)
        {
            oldText = tmp.text;
            originalText = tmp.text;


            string newString = originalText;
            if (string.IsNullOrEmpty(newString))
                return;

            newString = ResolveString(newString, Variables);

            // update both our textstring and the TMP string so we don't run an update
            tmp.text = newString;
            oldText = newString;
        }
    }
}
