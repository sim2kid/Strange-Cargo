// We receive data as raw HID input reports. This struct
// describes the raw binary format of such a report.
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

[StructLayout(LayoutKind.Explicit, Size = 32)]
struct StadiaGamepadInputReport : IInputStateTypeInfo
{
    // Because all HID input reports are tagged with the 'HID ' FourCC,
    // this is the format we need to use for this state struct.
    public FourCC format => new FourCC('H', 'I', 'D');

    // HID input reports can start with an 8-bit report ID. It depends on the device
    // whether this is present or not. On the PS4 DualShock controller, it is
    // present. We don't really need to add the field, but let's do so for the sake of
    // completeness. This can also help with debugging.
    [FieldOffset(0)] public byte reportId;

    // The InputControl annotations here probably look a little scary, but what we do
    // here is relatively straightforward. The fields we add we annotate with
    // [FieldOffset] to force them to the right location, and then we add InputControl
    // to attach controls to the fields. Each InputControl attribute can only do one of
    // two things: either it adds a new control or it modifies an existing control.
    // Given that our layout is based on Gamepad, almost all the controls here are
    // inherited from Gamepad, and we just modify settings on them.

    [InputControl(name = "leftStick", layout = "Stick", format = "VC2B")]
    [InputControl(name = "leftStick/x", offset = 0, format = "BYTE",
        parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
    [InputControl(name = "leftStick/left", offset = 0, format = "BYTE",
        parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp,clampMin=0,clampMax=0.5,invert")]
    [InputControl(name = "leftStick/right", offset = 0, format = "BYTE",
        parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp,clampMin=0.5,clampMax=1")]
    [InputControl(name = "leftStick/y", offset = 1, format = "BYTE",
        parameters = "invert,normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
    [InputControl(name = "leftStick/up", offset = 1, format = "BYTE",
        parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp,clampMin=0,clampMax=0.5,invert")]
    [InputControl(name = "leftStick/down", offset = 1, format = "BYTE",
        parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp,clampMin=0.5,clampMax=1,invert=false")]
    [FieldOffset(1)] public byte leftStickX;
    [FieldOffset(2)] public byte leftStickY;

    [InputControl(name = "rightStick", layout = "Stick", format = "VC2B")]
    [InputControl(name = "rightStick/x", offset = 0, format = "BYTE", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
    [InputControl(name = "rightStick/left", offset = 0, format = "BYTE", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp,clampMin=0,clampMax=0.5,invert")]
    [InputControl(name = "rightStick/right", offset = 0, format = "BYTE", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp,clampMin=0.5,clampMax=1")]
    [InputControl(name = "rightStick/y", offset = 1, format = "BYTE", parameters = "invert,normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
    [InputControl(name = "rightStick/up", offset = 1, format = "BYTE", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp,clampMin=0,clampMax=0.5,invert")]
    [InputControl(name = "rightStick/down", offset = 1, format = "BYTE", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp,clampMin=0.5,clampMax=1,invert=false")]
    [FieldOffset(3)] public byte rightStickX;
    [FieldOffset(4)] public byte rightStickY;

    [InputControl(name = "dpad", format = "BIT", layout = "Dpad", sizeInBits = 4, defaultState = 8)]
    [InputControl(name = "dpad/up", format = "BIT", layout = "DiscreteButton", parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7", bit = 0, sizeInBits = 4)]
    [InputControl(name = "dpad/right", format = "BIT", layout = "DiscreteButton", parameters = "minValue=1,maxValue=3", bit = 0, sizeInBits = 4)]
    [InputControl(name = "dpad/down", format = "BIT", layout = "DiscreteButton", parameters = "minValue=3,maxValue=5", bit = 0, sizeInBits = 4)]
    [InputControl(name = "dpad/left", format = "BIT", layout = "DiscreteButton", parameters = "minValue=5, maxValue=7", bit = 0, sizeInBits = 4)]
    [InputControl(name = "buttonWest", displayName = "Square", bit = 4)]
    [InputControl(name = "buttonSouth", displayName = "Cross", bit = 5)]
    [InputControl(name = "buttonEast", displayName = "Circle", bit = 6)]
    [InputControl(name = "buttonNorth", displayName = "Triangle", bit = 7)]
    [FieldOffset(5)] public byte buttons1;

    [InputControl(name = "leftShoulder", bit = 0)]
    [InputControl(name = "rightShoulder", bit = 1)]
    [InputControl(name = "leftTriggerButton", layout = "Button", bit = 2)]
    [InputControl(name = "rightTriggerButton", layout = "Button", bit = 3)]
    [InputControl(name = "select", displayName = "Share", bit = 4)]
    [InputControl(name = "start", displayName = "Options", bit = 5)]
    [InputControl(name = "leftStickPress", bit = 6)]
    [InputControl(name = "rightStickPress", bit = 7)]
    [FieldOffset(6)] public byte buttons2;

    [InputControl(name = "systemButton", layout = "Button", displayName = "System", bit = 0)]
    [InputControl(name = "touchpadButton", layout = "Button", displayName = "Touchpad Press", bit = 1)]
    [FieldOffset(7)] public byte buttons3;

    [InputControl(name = "leftTrigger", format = "BYTE")]
    [FieldOffset(8)] public byte leftTrigger;

    [InputControl(name = "rightTrigger", format = "BYTE")]
    [FieldOffset(9)] public byte rightTrigger;
}