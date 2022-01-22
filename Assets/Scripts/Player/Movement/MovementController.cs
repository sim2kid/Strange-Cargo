using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Movement
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
        [Tooltip("The layer mask the will be counted as ground.")]
        [SerializeField]
        LayerMask interactionMask = 0;

        CharacterController characterController;
        Vector2 moveValue;
        Vector3 moveTo;

        //references for handling gravity
        [SerializeField]
        Vector3 velocity;

        [SerializeField]
        private bool JumpIsHit;
        [SerializeField]
        private bool IsHeadHit;

        [SerializeField]
        bool _onGround;
        public bool IsOnGround { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
            characterController = GetComponent<CharacterController>();

            if (characterController == null)
                throw new MissingComponentException("Missing a CharacterController componenet");

            CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
            if (capsuleCollider != null) 
            {
                Console.DebugOnly(capsuleCollider.direction);
                characterController.center = capsuleCollider.center;
                characterController.radius = capsuleCollider.radius;
                characterController.height = capsuleCollider.height;
            }
            JumpIsHit = false;
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
            if (IsOnGround && !JumpIsHit)
            {
                velocity.y = jumpHeight * -Physics.gravity.y;//Mathf.Sqrt(jumpHeight * -2.0f * -gravityForce);
                JumpIsHit = true;
            }
        }

        void FixedUpdate()
        {
            HandleGravity();
            HandleMovement();
            _onGround = IsOnGround;
        }

        void HandleMovement()
        {
            moveTo = transform.right * moveValue.x + transform.forward * moveValue.y;
            characterController.Move(moveTo.normalized * Time.fixedDeltaTime * MoveSpeed);
        }

        float GetRadius() 
        {
            return characterController.radius * transform.lossyScale.y;
        }

        Vector3 GetHeadOrigin() 
        {
            float radius = GetRadius();
            Vector3 headOrigin = transform.position + characterController.center + (new Vector3(0, (characterController.height / 2) + 0.2f, 0) * transform.lossyScale.y);
            headOrigin -= new Vector3(0, radius, 0);
            return headOrigin;
        }

        Vector3 GetFootOrigin() 
        {
            float radius = GetRadius();
            Vector3 footOrigin = transform.position + characterController.center - (new Vector3(0, characterController.height / 2, 0) * transform.lossyScale.y);
            footOrigin += new Vector3(0, radius, 0);
            footOrigin += PhysicsStepResolution();
            return footOrigin;
        }

        Vector3 PhysicsStepResolution() 
        {
            return Physics.gravity * Time.fixedDeltaTime;
        }

        void HandleGravity()
        {
            float radius = GetRadius();

            IsOnGround = Physics.CheckSphere(GetFootOrigin(), radius, interactionMask, QueryTriggerInteraction.Ignore);
            IsHeadHit = Physics.CheckSphere(GetHeadOrigin(), radius, interactionMask, QueryTriggerInteraction.Ignore);

            if (IsHeadHit)
            {
                JumpIsHit = false;
                if (velocity.y > 0) 
                velocity.y = 0;
            }

            if (JumpIsHit && !IsOnGround)
                JumpIsHit = false;
            if(!IsOnGround)
                velocity.y += Physics.gravity.y * Time.fixedDeltaTime;

            characterController.Move(velocity * Time.fixedDeltaTime);
        }

        void OnDrawGizmosSelected()
        {
            if (characterController == null)
                characterController = GetComponent<CharacterController>();
            if (characterController == null)
                return;

            float radius = GetRadius();

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(GetHeadOrigin(), radius);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(GetFootOrigin() - PhysicsStepResolution(), radius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(GetFootOrigin(), radius);
        }
    }
}