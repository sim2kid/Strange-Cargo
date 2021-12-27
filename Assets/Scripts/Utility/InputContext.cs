using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.iOS;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.XInput;

namespace Utility.Input {
    public class InputContext : MonoBehaviour
    {
        private PlayerInput playerInput;

        [Tooltip("The default specific device. Good for UIs")]
        [SerializeField]
        public Device DefaultDeviceType = Device.Xbox;

        [SerializeField]
        public UnityEvent OnDeviceChange;

        [Header("Read Only Fields")]

        [Tooltip("(ReadOnly) The Broad Device Scheme being used.")]
        [SerializeField]
        private string _recentScheme;
        [Tooltip("(ReadOnly) The current specific device plugged in.")]
        [SerializeField]
        private Device _lastDevice;

        /// <summary>
        /// The current broad device scheme
        /// </summary>
        public Scheme CurrentScheme { get => ResolveScheme(_recentScheme); }

        /// <summary>
        /// The current specific device. Good for button types in UI.
        /// </summary>
        public Device CurrentDevice { get => ResolveDeviceType(CurrentScheme); }

        // Start is called before the first frame update
        void Start()
        {
            playerInput = GameObject.FindObjectOfType<PlayerInput>();
            if (playerInput == null)
            {
                Console.LogError("No PlayerInput component found in scene.");
                return;
            }
            _recentScheme = playerInput.currentControlScheme;
            _lastDevice = DefaultDeviceType;

            Console.Log($"Current Input: {_recentScheme}");
        }


        // Update is called once per frame
        void Update()
        {
            if (playerInput == null)
                return;

            if (playerInput.currentControlScheme != _recentScheme)
                OnNewInput();

            if(Application.isEditor && CurrentDevice != Device.Unknown)
                _lastDevice = CurrentDevice;
        }

        private void OnNewInput()
        {
            _recentScheme = playerInput.currentControlScheme;
            Console.Log($"New Input: {_recentScheme} : {CurrentDevice}");
            OnDeviceChange.Invoke();
        }

        public Device ResolveDeviceType(Scheme scheme) 
        {
            switch (scheme) 
            {
                case Scheme.KeyboardMouse:
                    _lastDevice = Device.Keyboard;
                    break;

                case Scheme.Gamepad:
                    Gamepad device = Gamepad.current;
                    if (device == null) 
                    {
                        Console.LogWarning($"Could not resolve the current gamepad. Is one plugged in?");
                        return Device.Unknown;
                    }
                    else if (IsOrSub(device.GetType(), typeof(DualShockGamepad)))
                    {
                        _lastDevice = Device.PlayStation;
                        break;
                    } 
                    else if (IsOrSub(device.GetType(), typeof(iOSGameController)))
                    {
                        _lastDevice = DefaultDeviceType;
                        break;
                    } 
                    else if (IsOrSub(device.GetType(), typeof(SwitchProControllerHID)))
                    {
                        _lastDevice = Device.Nintendo;
                        break;
                    } 
                    else if (IsOrSub(device.GetType(), typeof(XInputController)))
                    {
                        _lastDevice= Device.Xbox;
                        break; 
                    } 
                    else if (IsOrSub(device.GetType(), typeof(StadiaController)))
                    {
                        _lastDevice = Device.Stadia;
                        break;
                    }
                    Console.LogWarning($"Could not resolve gamepad of type {device.GetType()}. Will use the defaults, {DefaultDeviceType}.");
                    return DefaultDeviceType;

                case Scheme.Touch:
                    Console.Log($"Touch doesn not have it's own scheme. Will use the default, {DefaultDeviceType}.");
                    _lastDevice = DefaultDeviceType;
                    break;

                case Scheme.Joystick:
                case Scheme.XR:
                default:
                    Console.LogWarning($"Scheme {scheme.ToString()} is unsupported.");
                    return Device.Unknown;
            }

            return _lastDevice;
        }

        private static bool IsOrSub(System.Type first, System.Type second) 
        {
            return first.IsSubclassOf(second) || first == second;
        }

        /// <summary>
        /// Will return the device type based off of the <paramref name="scheme"/>
        /// </summary>
        /// <param name="scheme"></param>
        /// <returns></returns>
        public static Scheme ResolveScheme(string scheme) 
        {
            switch (scheme.ToLower())
            {
                case "mouse":
                case "keyboard":
                case "keyboardmouse":
                case "keyboard&mouse":
                    return Scheme.KeyboardMouse;
                case "gamepad":
                    return Scheme.Gamepad;
                case "joystick":
                    return Scheme.Joystick;
                case "touch":
                    return Scheme.Touch;
                case "xr":
                case "vr":
                case "ar":
                    return Scheme.XR;
                default:
                    Console.LogError($"{scheme} is an Unknown device type. You might want to add it to the Input Context." +
                        $"If you're using a custom device you wish to be supported, please ask the devs.");
                    return Scheme.Unknown;
            }    
        }

        public static List<string> InputsForAction(InputAction action)
        {
            // Create Array String seperated by ','
            string actionName = action.ToString();
            actionName = actionName.Substring(actionName.IndexOf('[') + 1);
            actionName = actionName.Substring(0, actionName.Length - 1);

            string[] inputNames = actionName.Split(',');
            List<string> outputNames = new List<string>();

            for (int i = 0; i < inputNames.Length; i++)
            {
                // skip the device name.
                string output = string.Empty;
                inputNames[i] = inputNames[i].Substring(1);
                outputNames.Add(inputNames[i].Substring(inputNames[i].IndexOf('/') + 1));
            }

            return outputNames;
        }

        public InputAction GetAction(string actionName) 
        {
            string name = actionName;
            InputAction a = playerInput.actions[name];
            if (a == null)
            {
                name = char.ToLower(name[0]) + name.Substring(1);
                a = playerInput.actions[name];
            }
            else
                return a;

            if (a == null)
                a = playerInput.actions[name.ToLower()];
            else
                return a;

            if (a == null)
                Console.LogWarning($"Action \"{actionName}\" was not found in the selected PlayerInput modual.");
            return a;
        }
    }

    public enum Device 
    {
        Keyboard,
        Stadia,
        Xbox,
        PlayStation,
        Nintendo,
        Unknown
    }

    public enum Scheme 
    {
        KeyboardMouse,
        Gamepad,
        Joystick,
        Touch,
        XR,
        Unknown
    }
}