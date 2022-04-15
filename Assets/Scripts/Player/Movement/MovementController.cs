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
        [Tooltip("The speed of the player.")]
        [Range(1f, 100f)]
        [SerializeField]
        public float MoveSpeed = 10.0f;
        [Tooltip("The speed of the player.")]
        [Range(1f, 100f)]
        [SerializeField]
        public float CrouchingMoveSpeed = 5.0f;
        [Tooltip("The height of a jump")]
        [Range(0.0f, 5.0f)]
        [SerializeField]
        float jumpHeight = 2.0f;
        [Tooltip("The layer mask the will be counted as ground.")]
        [SerializeField]
        LayerMask interactionMask = 0;
        public LayerMask LayerMask { get => interactionMask; }
        [Tooltip("Normal altitude of player's head.")]
        public Vector3 headPositionDefault = new Vector3(0, 0.5649999f, 0);
        [Tooltip("Altitude of player's head while crouched.")]
        public Vector3 headPositionCrouched = Vector3.zero;
        [Tooltip("The amount of time it takes the camera to go from crouched to uncrouched or vice versa.")]
        public float timeToCrouch = 0.125f;
        [Tooltip("The time at which the player started crouching or uncrouching.")]
        private float crouchStartTime;
        [Tooltip("The game object which contains the player's head and lookat location")]
        public GameObject eyes;
        [Tooltip("The game object representing the center of the head. This part doesn't rotate.")]
        public GameObject headGroup;
        [Tooltip("Is the crouch button currently being held down?")]
        private bool isCrouching = false;

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
        private Vector3 _movementSpeed;
        public Vector3 Velocity { get => new Vector3(_movementSpeed.x, velocity.y, _movementSpeed.z); }

        // Start is called before the first frame update
        void Start()
        {
            characterController = GetComponent<CharacterController>();

            if (characterController == null)
                throw new MissingComponentException("Missing a CharacterController componenet");

            CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
            if (capsuleCollider != null) 
            {
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

        void OnCrouch(InputValue value)
        {
            if(value.isPressed)
            {
                isCrouching = true;
            }
            else
            {
                isCrouching = false;
            }
            crouchStartTime = Time.time;
        }

        void HandleCrouching()
        {
            float fracComplete = (Time.time - crouchStartTime) / timeToCrouch;
            if(isCrouching)
            {
                headGroup.transform.localPosition = Vector3.Lerp(headPositionDefault, headPositionCrouched, fracComplete);
            }
            else if(!isCrouching)
            {
                headGroup.transform.localPosition = Vector3.Lerp(headPositionCrouched, headPositionDefault, fracComplete);
            }
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
            MoveSpeed = Settings.Instance.Values.PlayerSpeed;
            CrouchingMoveSpeed = MoveSpeed / 2;
            HandleGravity();
            HandleMovement();
            HandleCrouching();
            _onGround = IsOnGround;
        }

        void HandleMovement()
        {
            float speed = isCrouching ? CrouchingMoveSpeed : MoveSpeed;

            moveTo = transform.right * moveValue.x + transform.forward * moveValue.y;
            _movementSpeed = moveTo.normalized * Time.fixedDeltaTime * speed;
            characterController.Move(_movementSpeed);
        }

        public float GetRadius() 
        {
            return characterController.radius * transform.lossyScale.y;
        }

        public Vector3 GetHeadOrigin() 
        {
            float radius = GetRadius();
            Vector3 headOrigin = transform.position + characterController.center + (new Vector3(0, (characterController.height / 2) + 0.2f, 0) * transform.lossyScale.y);
            headOrigin -= new Vector3(0, radius, 0);
            return headOrigin;
        }

        public Vector3 GetFootOrigin() 
        {
            float radius = GetRadius();
            Vector3 footOrigin = transform.position + characterController.center - (new Vector3(0, characterController.height / 2, 0) * transform.lossyScale.y);
            footOrigin += new Vector3(0, radius, 0);
            footOrigin += PhysicsStepResolution();
            return footOrigin;
        }

        public Vector3 PhysicsStepResolution() 
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
            if (JumpIsHit && velocity.y == 0) 
                HandleJumping();
            if (JumpIsHit && !IsOnGround)
                JumpIsHit = false;
            if(!IsOnGround)
                velocity.y += Physics.gravity.y * Time.fixedDeltaTime;
            if (characterController.isGrounded)
                velocity.y = 0;

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