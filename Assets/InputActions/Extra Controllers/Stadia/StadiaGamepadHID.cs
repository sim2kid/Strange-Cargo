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

        // Alternatively, you can also match by PID and VID, which is generally
        // more reliable for HIDs.
        InputSystem.RegisterLayout<StadiaGamepadHID>(
            matches: new InputDeviceMatcher()
                .WithInterface("HID")
                .WithCapability("vendorId", 0x54C) // Sony Entertainment.
                .WithCapability("productId", 0x9CC)); // Wireless controller.
    }

    // In the Player, to trigger the calling of the static constructor,
    // create an empty method annotated with RuntimeInitializeOnLoadMethod.
    [RuntimeInitializeOnLoadMethod]
    static void Init()
    { }
}