using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[DisallowMultipleComponent]
public class MouseLooker : MonoBehaviour
{
    [Tooltip("Invert the Vertical input (Usually on)")]
    [SerializeField]
    public bool invertYAxis = true;
    [Tooltip("Invert the Horizontal input (Usually off)")]
    [SerializeField]
    public bool invertXAxis = false;
    [Tooltip("Hoe fast the camera will turn")]
    [SerializeField] 
    private float mouseSensitivity;
    [Tooltip("The thing that will turn virtically (The game camera)")]
    [SerializeField] 
    private new GameObject camera;

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
            look = playerInput.actions["Look"]; // This is our Vector2 with the mouse movement
        }
    }

    private void Update()
    {
        MouseLook();
    }

    private void MouseLook()
    {
        // The rotation of the current gameobjects we're manipulating
        Vector2 rotation = new Vector2(transform.localEulerAngles.x, camera.transform.localEulerAngles.y); 

        // Where we want to go based on player input
        Vector2 targetChange = look.ReadValue<Vector2>() * mouseSensitivity * Time.deltaTime;
        targetChange.y = Mathf.Clamp(targetChange.y, -90, 90); // Clamp the vertical to up and down
        // Invert any axis that need to be inverted
        if(invertYAxis)
            targetChange.y = -targetChange.y;
        if (invertXAxis)
            targetChange.x = -targetChange.x;

        // Add those onto our current rotation
        rotation += targetChange;

        // Update our gameobjects
        transform.localEulerAngles = new Vector3(0, rotation.x, 0);
        camera.transform.localEulerAngles = new Vector3(rotation.y,0, 0);
    }
}
