using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10.0f;
    [SerializeField] float gravityForce = 1.0f;
    [SerializeField] float jumpHeight = 2.0f;
    CharacterController characterController;
    Vector2 moveValue;
    Vector3 moveTo;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();

        moveTo = transform.right * moveValue.x + transform.forward * moveValue.y;
    }

    void OnJump()
    {
        HandleJumping();
    }

    void HandleJumping()
    {
        if (characterController.isGrounded)
        {
            moveTo.y = jumpHeight;
        }
    }

    void FixedUpdate()
    {
        //HandleGravity();
        HandleMovement();
    }

    void HandleMovement()
    {
        characterController.Move(moveTo.normalized * Time.fixedDeltaTime * moveSpeed);
    }

    void HandleGravity()
    {
        moveTo.y -= gravityForce * Time.fixedDeltaTime; //TODO: Mend or outright replace gravity logic; current logic causes player to gradually slow to a crawl the longer a movement key is held
    }
}
