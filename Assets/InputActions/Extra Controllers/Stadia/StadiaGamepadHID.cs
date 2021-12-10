using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

[InputControlLayout(stateType = typeof(StadiaGamepadInputReport))]
#if UNITY_EDITOR
[InitializeOnLoad] // Make sure static constructor is called during startup.
#endif
public class StadiaGamepadHID : Gamepad
{
    static StadiaGamepadHID()
    {
        // This is one way to match the Device.
        InputSystem.RegisterLayout<StadiaGamepadHID>(
            "Google Stadia Controller",
            new InputDeviceMatcher()
                .WithInterface("HID")
                .WithManufacturer("Google Inc.")
                .WithProduct("Stadia Controller"));
    }

    // In the Player, to trigger the calling of the static constructor,
    // create an empty method annotated with RuntimeInitializeOnLoadMethod.
    [RuntimeInitializeOnLoadMethod]
    static void Init()
    { }
}