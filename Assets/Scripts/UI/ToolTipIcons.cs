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
        originalText = oldText = text.text;

        onNewDevice();
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
        var asset = GetAsset(context.CurrentDevice);
        if (asset != null)
            text.spriteAsset = asset;

        // rewrite current sprites for newly bound assets
        RewriteSprites();
    }

    private Dictionary<string, string> GetActionOptions(string vari) 
    {
        string[] str = vari.Split(' ');
        Dictionary<string, string> options = new Dictionary<string, string>();

        for (int i = 0; i < str.Length; i++) 
        {
            if (i == 0)
            {
                options.Add("name", str[0]);
                continue;
            }
            string[] keyvalue = str[i].Split('=');
            if (keyvalue.Length != 2)
                continue;
            if (options.ContainsKey(keyvalue[0]))
            {
                Console.LogWarning($"The option \"{keyvalue[0]}\" has been included twice in an action for {options["name"]}. All subsuquent options will be ignored. ");
                continue;
            }
            options.Add(keyvalue[0].ToLower(), keyvalue[1]);
        }
        return options;
    }

    private void RewriteSprites() 
    {
        string newString = originalText;
        if (string.IsNullOrEmpty(newString))
            return;
        // look at input actions in text
        // eg: "press {use} to eat"

        MatchCollection matches = Regex.Matches(newString, @"(?<=\{).*?(?=\})");
        List<string> matchList = matches.Cast<Match>().Select(match => match.Value).ToList();

        string device = context.CurrentDevice.ToString();
        string scheme = context.CurrentScheme.ToString();
        foreach (string vari in matchList)
        {
            var options = GetActionOptions(vari);
            string originalName = $"{{{vari}}}";

            // find the related button(s)
            // GetButtonForAction**
            var action = context.GetAction(options["name"]);


            string output = string.Empty;
            if (action == null)
            {
                output = $"{{{options[name]} unknown}}";
            }
            else 
            {
                List<string> actionNames = InputContext.InputsForAction(action);
                int startIndex = 0;
                int endIndex = actionNames.Count;
                float tint = 1f;

                if (options.TryGetValue("start", out string start))
                    if (int.TryParse(start, out int s))
                        startIndex = Mathf.Max(startIndex, Mathf.Min(s, endIndex));
                if (options.TryGetValue("end", out string end))
                {
                    if (int.TryParse(end, out int e))
                        endIndex = Mathf.Min(endIndex, Mathf.Max(e, startIndex));
                }
                else if (options.TryGetValue("length", out string length))
                {
                    if (int.TryParse(length, out int l))
                        endIndex = Mathf.Min(endIndex, Mathf.Max(startIndex + l, startIndex));
                }

                if (options.TryGetValue("index", out string index))
                    if (int.TryParse(index, out int id))
                    {
                        startIndex = Mathf.Min(0, Mathf.Max(id, actionNames.Count));
                        endIndex = Mathf.Max(actionNames.Count, Mathf.Min(id + 1, 0));
                    }

                if(options.TryGetValue("tint", out string tintstr))
                    if(float.TryParse(tintstr, out float t))
                        tint = Mathf.Clamp(t, 0f, 1f);

                // Can't find the sprite in spritesheet because not
                // all symbols will have unicode to reference. 
                // Instead we'll have to derive the name based on the input!

                // Grab the name and replace the action in string with it
                // eg: "press <sprite="keyboard" name="e"> to eat"

                for (int i = startIndex; i < endIndex; i++)
                {
                    output += $"<sprite=\"{device}\" name=\"{actionNames[i]}\" tint={tint}>";
                }

                // check if this should be kept

                string targetDevice = string.Empty;
                string targetScheme = string.Empty;

                if (options.TryGetValue("device", out string d))
                    targetDevice = d;
                if (options.TryGetValue("scheme", out string sch))
                {
                    if (sch.StartsWith("!"))
                    {
                        sch = sch.Substring(1);
                        targetScheme = "!" + InputContext.ResolveScheme(sch).ToString();

                    }
                    else
                    {
                        targetScheme = InputContext.ResolveScheme(sch).ToString();
                    }
                }

                if (!string.IsNullOrWhiteSpace(targetDevice)) 
                {
                    if (targetDevice.StartsWith("!"))
                    {
                        if (targetDevice.ToLower().Substring(1) == device.ToLower()) // Exclude decives
                            output = string.Empty;
                    }
                    else if (targetDevice.ToLower() != device.ToLower()) // inclue devices
                        output = string.Empty;
                }

                if (!string.IsNullOrWhiteSpace(targetScheme)) {
                    if (targetScheme.StartsWith("!")) {
                        if (targetScheme.ToLower().Substring(1) == scheme.ToLower()) // exclude schemes
                            output = string.Empty;
                    }
                    else if (targetScheme.ToLower() != scheme.ToLower()) // Include schemes
                        output = string.Empty;
                }
            }
            newString = newString.Replace(originalName, output);
        }

        // update both our textstring and the TMP string so we don't run an update
        text.text = newString;
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
