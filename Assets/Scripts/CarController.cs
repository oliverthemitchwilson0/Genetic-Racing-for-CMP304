using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    public float acceleration = 3.0f;
    public float maxDriveSpeed = 10.0f;
    public float steeringSpeedDegrees = 120.0f;
    public InputAction steering;
    public InputAction throttle;

    private Rigidbody2D rb;
    private float speed = 0.0f;

    public void OnEnable()
    {
        steering.Enable();
        throttle.Enable();
    }

    public void OnDisable()
    {
        steering.Disable();
        throttle.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float steerInput = steering.ReadValue<float>();
        rb.MoveRotation(rb.rotation - steeringSpeedDegrees * Time.fixedDeltaTime * steerInput);

        float throttleInput = throttle.ReadValue<float>();
        speed += (acceleration * Time.fixedDeltaTime) * throttleInput;
        speed = Mathf.Clamp(speed, -maxDriveSpeed / 2, maxDriveSpeed);
        rb.linearVelocity = transform.up * speed;
    }
}
