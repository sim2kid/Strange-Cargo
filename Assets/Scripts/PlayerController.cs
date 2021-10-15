using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //references for handling movement
    [SerializeField] float moveSpeed = 10.0f;
    CharacterController characterController;
    Vector2 moveValue;
    Vector3 moveTo;

    //references for handling gravity
    Vector3 velocity;
    [SerializeField] float gravityForce = 1.0f;
    [SerializeField] Transform feet;
    [SerializeField] float groundCheckDistance;
    [SerializeField] LayerMask groundMask;
    [Tooltip("Reset the player's downward velocity to this number when landing from a fall or jump (float).")]
    [SerializeField] float groundCheckVelocityReset = -2.0f;
    bool isOnGround;
    float jumpForceFormula;

    //references for handling jumping
    [SerializeField] float jumpHeight = 2.0f;

    //references for handling interaction
    [SerializeField] float cameraRaycastDistance = 10.0f;
    private RaycastHit cameraRaycastHit;
    private RaycastHit[] cameraRaycastHits;
    private List<float> cameraRaycastHitDistances;

    // Start is called before the first frame update
    void Start()
    {
        InitializeVars();
    }

    void InitializeVars()
    {
        characterController = GetComponent<CharacterController>();
        jumpForceFormula = Mathf.Sqrt(jumpHeight * -2.0f * -gravityForce);
        cameraRaycastHitDistances = new List<float>();
    }

    void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();
    }

    void OnJump()
    {
        HandleJumping();
    }

    void HandleJumping()
    {
        if (isOnGround)
        {
            velocity.y = jumpForceFormula;
        }
    }

    void FixedUpdate()
    {
        HandleGravity();
        HandleMovement();
    }

    void HandleMovement()
    {
        moveTo = transform.right * moveValue.x + transform.forward * moveValue.y;
        characterController.Move(moveTo.normalized * Time.fixedDeltaTime * moveSpeed);
    }

    void HandleGravity()
    {
        isOnGround = Physics.CheckSphere(feet.position, groundCheckDistance, groundMask);
        if(isOnGround && velocity.y < 0)
        {
            velocity.y = groundCheckVelocityReset;
        }
        velocity.y -= gravityForce * Time.fixedDeltaTime;
        characterController.Move(velocity * Time.fixedDeltaTime);
    }

    void OnInteract()
    {
        /*if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out cameraRaycastHit, cameraRaycastDistance))
        {
            if(cameraRaycastHit.transform.gameObject.TryGetComponent(out Interactable interactable))
            {
                interactable.Interact();
            }
        }*/


        cameraRaycastHits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, cameraRaycastDistance);
        foreach(RaycastHit hit in cameraRaycastHits)
        {
            cameraRaycastHitDistances.Add(hit.distance);
        }
        cameraRaycastHitDistances.Sort();
        foreach(RaycastHit hit in cameraRaycastHits)
        {
            if(hit.distance == cameraRaycastHitDistances[0])
            {
                if(hit.transform.gameObject.TryGetComponent(out Interactable interactable))
                {
                    interactable.Interact();
                    break;
                }
            }
        }
    }
}
