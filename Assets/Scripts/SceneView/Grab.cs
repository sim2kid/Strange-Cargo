using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InSceneView
{
    public class Grab : MonoBehaviour
    {
        public float sensitivity = 20f;
        public Vector2 screenPos;
        Utility.Input.InputContext context;
        PlayerInput pi;
        InputAction move;

        // Start is called before the first frame update
        void Start()
        {
            pi = FindObjectOfType<PlayerInput>();
            move = pi.actions["Move"];
            context = FindObjectOfType<Utility.Input.InputContext>();
        }

        // Update is called once per frame
        void Update()
        {
            if (context.CurrentScheme == Utility.Input.Scheme.KeyboardMouse)
            {
                screenPos = new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue());
            }
            else 
            {
                screenPos += move.ReadValue<Vector2>() * sensitivity;
            }
        }
    }
}