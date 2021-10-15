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

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        jumpForceFormula = Mathf.Sqrt(jumpHeight * -2.0f * -gravityForce);
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
        //TODO: Insert logic to run whenever the Interact key is pressed
    }
}
