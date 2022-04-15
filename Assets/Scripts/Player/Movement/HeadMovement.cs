using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Player.Movement
{
    [RequireComponent(typeof(PlayerInput))]
    [DisallowMultipleComponent]
    public class HeadMovement : MonoBehaviour
    {
        [Tooltip("Invert the Vertical input (Usually on)")]
        [SerializeField]
        public bool InvertYAxis = true;
        [Tooltip("Invert the Horizontal input (Usually off)")]
        [SerializeField]
        public bool InvertXAxis = false;
        [Tooltip("Hoe fast the camera will turn")]
        [Range(1, 150)]
        [SerializeField] 
        public float MouseSensitivity = 50;
        [Tooltip("The thing that will turn vertically (The game camera)")]
        [SerializeField] 
        public new GameObject camera;

        
        public UnityEvent OnJolt;

        private Vector2 lastInput;
        private Vector2 lastVector;

        private PlayerInput playerInput;
        private InputAction look;

        private void OnEnable()
        {
            // On enable, we make sure we have a camera
            if (camera == null)
                throw new ArgumentNullException("Camera must be set to a gameobject");
            // Then we lock the mouse while enabled
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnDisable()
        {
            // On disable, we free the mouse
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void Start()
        {
            // On start, we'll grab the player input info
            if (playerInput == null)
            {
                playerInput = GetComponent<PlayerInput>();
                if(playerInput == null)
                    throw new MissingComponentException("Missing a PlayerInput componenet");
                look = playerInput.actions["Look"]; // This is our Vector2 with the mouse movement
            }
        }

        private void Update()
        {
            MouseSensitivity = Settings.Instance.Values.MouseSensitivity;
            MouseLook();
        }

        private void MouseLook()
        {
            // The rotation of the current gameobjects we're manipulating
            Vector2 rotation = new Vector2(transform.localEulerAngles.y, camera.transform.localEulerAngles.x);

            Vector2 mouseInput = look.ReadValue<Vector2>();

            if (mouseInput != lastInput && mouseInput.magnitude > 40 && mouseInput.magnitude < 200)
            {
                Vector2 thisVector = mouseInput - lastInput;

                float f = Vector2.Angle(thisVector.normalized, lastVector.normalized);

                if (f < 20 && f != 0)
                    OnJolt.Invoke();
                lastInput = mouseInput;
                lastVector = thisVector;
            }

            // Where we want to go based on player input
            Vector2 targetChange = look.ReadValue<Vector2>() * MouseSensitivity * Time.deltaTime;

            // Invert any axis that need to be inverted
            if (InvertYAxis)
                targetChange.y = -targetChange.y;
            if (InvertXAxis)
                targetChange.x = -targetChange.x;

            // Add those onto our current rotation
            rotation += targetChange;

            if (rotation.y > 180)
                rotation.y -= 360;
            rotation.y = Mathf.Clamp(rotation.y, -90, 90);

            // Update our gameobjects
            transform.localEulerAngles = new Vector3(0, rotation.x, 0);
            camera.transform.localEulerAngles = new Vector3(rotation.y,0, 0);
        }
    }
}