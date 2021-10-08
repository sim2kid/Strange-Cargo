using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLooker : MonoBehaviour
{
    [SerializeField] private Vector2 mouseSensitivity;
    [SerializeField] private Vector2 mouseAcceleration;
    [SerializeField] private float mouseInputLagPeriod;
    [SerializeField] private float maxVerticalAngleFromHorizon;
    [SerializeField] RotationDirection rotationDirections;
    private Vector2 mouseVelocity;
    private Vector2 rotation;
    private Vector2 lastMouseInputEvent;
    private float mouseInputLagTimer;
    [Flags]
    public enum RotationDirection
    {
        None,
        Horizontal = (1 << 0),
        Vertical = (1 << 1)
    }

    private void OnEnable()
    {
        ResetMouseLook();
    }

    private void Update()
    {
        MouseLook();
    }
    Vector2 GetMouseInput()
    {
        mouseInputLagTimer += Time.deltaTime;
        Vector2 mouseInput = new Vector2(Mouse.current.delta.x.ReadValue(), Mouse.current.delta.y.ReadValue());
        if ((Mathf.Approximately(0, mouseInput.x) && Mathf.Approximately(0, mouseInput.y)) == false || mouseInputLagTimer >= mouseInputLagPeriod)
        {
            lastMouseInputEvent = mouseInput;
            mouseInputLagTimer = 0;
        }
        return lastMouseInputEvent;
    }

    void MouseLook()
    {
        Vector2 targetMouseVelocity = GetMouseInput() * mouseSensitivity;
        if ((rotationDirections & RotationDirection.Horizontal) == 0)
        {
            targetMouseVelocity.x = 0;
        }
        if ((rotationDirections & RotationDirection.Vertical) == 0)
        {
            targetMouseVelocity.y = 0;
        }
        mouseVelocity = new Vector2(Mathf.MoveTowards(mouseVelocity.x, targetMouseVelocity.x, mouseAcceleration.x * Time.deltaTime), Mathf.MoveTowards(mouseVelocity.y, targetMouseVelocity.y, mouseAcceleration.y * Time.deltaTime));
        rotation += mouseVelocity * Time.deltaTime;
        rotation.y = ClampVerticalAngle(rotation.y);
        transform.localEulerAngles = new Vector3(rotation.y, rotation.x, 0);
    }

    void ResetMouseLook()
    {
        mouseVelocity = Vector2.zero;
        mouseInputLagTimer = 0;
        lastMouseInputEvent = Vector2.zero;
        Vector3 euler = transform.localEulerAngles;
        if (euler.x >= 180)
        {
            euler.x -= 360;
        }
        euler.x = ClampVerticalAngle(euler.x);
        transform.localEulerAngles = euler;
        rotation = new Vector2(euler.y, euler.x);
    }

    float ClampVerticalAngle(float angle)
    {
        return Mathf.Clamp(angle, -maxVerticalAngleFromHorizon, maxVerticalAngleFromHorizon);
    }
}
