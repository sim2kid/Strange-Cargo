using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerController
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(CharacterController))]
    [DisallowMultipleComponent]
    public class MovementController : MonoBehaviour
    {
        // Exposed fields
        [Tooltip("The speef of the player.")]
        [Range(1f, 100f)]
        [SerializeField]
        public float MoveSpeed = 10.0f;
        [Tooltip("The height of a jump")]
        [Range(0.0f, 5.0f)]
        [SerializeField]
        float jumpHeight = 2.0f;
        [Tooltip("The force of Gravity (abstracted version)")]
        [SerializeField]
        float gravityForce = 1.0f;
        [Tooltip("The tranform of the bottom of the collision box.")]
        [SerializeField]
        Transform feet;
        [Tooltip("The distance the player controller will check for ground.")]
        [SerializeField]
        float groundCheckDistance;
        [Tooltip("The layer mask the will be counted as ground.")]
        [SerializeField]
        LayerMask groundMask;
        [Tooltip("Reset the player's downward velocity to this number when landing from a fall or jump (float).")]
        [SerializeField]
        float groundCheckVelocityReset = -2.0f;

        CharacterController characterController;
        Vector2 moveValue;
        Vector3 moveTo;

        //references for handling gravity
        Vector3 velocity;

        public bool isOnGround;
        float jumpForceFormula;

        //references for handling jumping


        // Start is called before the first frame update
        void Start()
        {
            if (feet == null)
                throw new ArgumentNullException("The Feet Transform is required for this to work properly.");

            characterController = GetComponent<CharacterController>();

            if (characterController == null)
                throw new MissingComponentException("Missing a CharacterController componenet");

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
            characterController.Move(moveTo.normalized * Time.fixedDeltaTime * MoveSpeed);
        }

        void HandleGravity()
        {
            isOnGround = Physics.CheckSphere(feet.position, groundCheckDistance, groundMask);
            if (isOnGround && velocity.y < 0)
            {
                velocity.y = groundCheckVelocityReset;
            }
            velocity.y -= gravityForce * Time.fixedDeltaTime;
            characterController.Move(velocity * Time.fixedDeltaTime);
        }
    }
}