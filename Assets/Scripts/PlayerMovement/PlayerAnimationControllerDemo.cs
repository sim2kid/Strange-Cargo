using PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(MovementController))]
public class PlayerAnimationControllerDemo : MonoBehaviour
{
    private CharacterController characterController;
    private Vector2 groundVelocity;
    private Animator animator;
    private MovementController movementController;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        movementController = GetComponent<MovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        groundVelocity = new Vector2(characterController.velocity.x, characterController.velocity.z);
        animator.SetFloat("GroundSpeed", groundVelocity.magnitude);
        if (!movementController.isOnGround)
        {
            animator.SetFloat("VerticalSpeed", characterController.velocity.y);
        }
    }
}
