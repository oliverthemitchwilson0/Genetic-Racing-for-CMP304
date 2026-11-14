using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

//Car Controlling Adapted from: https://www.youtube.com/watch?v=d7IY7mJECRk
public class CarController : MonoBehaviour
{
    //public bool debugControls = false;
    //public InputAction steering;
    //public InputAction throttle;
    public float steeringInput = 0f;
    public float throttleInput = 0f;
    public float acceleration = 3.0f;
    public float friction = 1f;
    public float maxDriveSpeed = 10.0f;
    public float steerStrength = 120.0f;
    public AnimationCurve steeringBySpeed;

    private Rigidbody2D rb;
    public float speed = 0.0f;
    private bool active = true;

    public void OnEnable()
    {
        //steering.Enable();
        //throttle.Enable();
    }

    public void OnDisable()
    {
        //steering.Disable();
        //throttle.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        //float throttleInput = throttle.ReadValue<float>();
        if (throttleInput > 0.0f)
            speed += (acceleration * Time.fixedDeltaTime);
        else
            speed -= friction * Time.fixedDeltaTime;

        speed = Mathf.Clamp(speed, 0f, maxDriveSpeed);
        rb.linearVelocity = transform.up * speed;

        float speedFraction = speed / maxDriveSpeed;
        float steerFactor = steeringBySpeed.Evaluate(speedFraction) * steerStrength;
        //float steerInput = steering.ReadValue<float>();
        rb.MoveRotation(rb.rotation - steerFactor * Time.fixedDeltaTime * steeringInput);
    }
    private void Update()
    {
        if (!active)
            DestroyImmediate(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        active = false;
    }
}
