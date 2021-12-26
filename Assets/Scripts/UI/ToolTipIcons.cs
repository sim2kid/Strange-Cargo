using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Utility.Input;
using UnityEngine.InputSystem;
using System.Text.RegularExpressions;
using System.Linq;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ToolTipIcons : MonoBehaviour
{
    TextMeshProUGUI text;
    InputContext context;

    string originalText;
    string oldText;

    void Start()
    {
        context = FindObjectOfType<InputContext>();
        if (context == null)
        {
            Console.LogError("Missing InputContext component in scene. Will delete ToolTipIcon script to prevent errors");
            Destroy(this);
        }
        text = GetComponent<TextMeshProUGUI>();
        context.OnDeviceChange.AddListener(onNewDevice);
        onNewDevice();
        originalText = oldText = text.text;
    }

    private string GetButtonForAction(string actionName) 
    {
        if(!TryGetBinding(actionName, out InputBinding result))
            return string.Empty;
        string[] schemeButton = result.path.Split('/');
        string scheme = schemeButton[0];
        string button = schemeButton[1];
        for (int i = 2; i < schemeButton.Length; i++)
            button += $"-{schemeButton[i]}";

        return button;
    }

    private bool TryGetBinding(string actionName, out InputBinding result) 
    {
        result = new InputBinding();
        InputAction action = null;
        try
        {
            PlayerInput p = FindObjectOfType<PlayerInput>();
            if (p == null)
            {
                Console.LogError("There is no active player input in the scene!!");
                return false;
            }
            action = p.actions[actionName];
        }
        catch 
        {
            Console.LogWarning($"{actionName} is not a valid action.");
            return false;
        }
        if (action == null) 
        {
            Console.LogWarning($"{actionName} is not a valid action.");
            return false;
        }

        foreach (InputBinding binding in action.bindings.ToArray()) 
        {
            string[] schemeButton = binding.path.Split('/');
            string scheme = schemeButton[0];

            if(InputContext.ResolveScheme(scheme) == context.CurrentScheme) 
            {
                result = binding;
                return true;
            }
        }

        return false;
    } 

    private void OnEnable()
    {
        if(context != null)
            context.OnDeviceChange.AddListener(onNewDevice);
    }

    private void OnDisable()
    {
        context.OnDeviceChange.RemoveListener(onNewDevice);
    }

    private void onNewDevice() 
    {
        Console.DebugOnly(GetButtonForAction("Pause"));

        var asset = GetAsset(context.CurrentDevice);
        if (asset != null)
            text.spriteAsset = asset;

        // rewrite current sprites for newly bound assets
        RewriteSprites();
    }

    private void RewriteSprites() 
    {
        string newString = originalText;

        // look at input actions in text
        // eg: "press {use} to eat"

        MatchCollection matches = Regex.Matches(newString, @"(?<=\{).*?(?=\})");
        List<string> matchList = matches.Cast<Match>().Select(match => match.Value).ToList();
        List<string> originalNames = new List<string>();
        List<string> actions = new List<string>();

        string device = context.CurrentDevice.ToString();
        foreach (string vari in matchList)
        {
            originalNames.Add($"{{{vari}}}");
            actions.Add(context.GetAction(vari).ToString());
        }

        for (int i = 0; i < actions.Count; i++) 
        {
            string output = string.Empty;
            if (actions[i] == null)
            {
                output = originalNames[i];
            }
            else
            {
                string actionName = actions[i].ToString();
                actionName = actionName.Substring(actionName.IndexOf('[') + 1);
                actionName = actionName.Substring(1, actionName.Length - 2);
                actionName = actionName.Substring(actionName.IndexOf('/') + 1);

                output = $"<sprite=\"{device}\" name=\"{actionName}\">";
            }

            newString = newString.Replace(originalNames[i], output);
        }

        text.text = newString;

        // find the related button
        // GetButtonForAction**

        // Can't find the sprite in spritesheet because not
        // all symbols will have unicode to reference. 
        // Instead we'll have to derive the name based on the input!

        // Grab the ID and replace the action in string with it
        // eg: "press <sprite="keyboard" name="e"> to eat"

        oldText = newString;
    }

    private void onTextChange() 
    {
        RewriteSprites();
    }

    private static TMP_SpriteAsset GetAsset(Device device) 
    {
        string resourcePath = "Inputs";
        switch (device)
        {
            default:
            case Device.Keyboard:
                return Resources.Load<TMP_SpriteAsset>($"{resourcePath}/Keyboard");

        }
    }


    void Update()
    {
        if (text.text != oldText) 
        {
            oldText = text.text;
            originalText = text.text;
            onTextChange();
        }
    }
}
