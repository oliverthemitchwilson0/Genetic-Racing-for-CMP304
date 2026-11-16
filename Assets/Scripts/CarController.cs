using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

//Car Controlling Adapted from: https://www.youtube.com/watch?v=d7IY7mJECRk
public class CarController : MonoBehaviour
{
    public float acceleration = 3.0f;
    public float friction = 1f;
    public float maxSpeed = 10.0f;
    public float steeringStrength = 120.0f;
    public AnimationCurve steerignCurve;

    private Rigidbody2D rigidBody;

    private float currentSpeed = 0f;
    private bool isActive = true;

    private float steeringInput = 0f;
    private float throttleInput = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Throttle();
        Steering();
    }

    private void Update()
    {
        if (!isActive)
            DestroyImmediate(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isActive = false;
    }

    private void Throttle()
    {
        if (throttleInput > 0.0f)   //Throttle Positive: Accelerate.
            currentSpeed += (acceleration * Time.fixedDeltaTime);
        else                        //Throttle anything else: apply friction.
            currentSpeed -= friction * Time.fixedDeltaTime;

        //Possibly add braking for negative throttleInput.

        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
        rigidBody.linearVelocity = transform.up * currentSpeed;
    }

    private void Steering()
    {
        float speedFraction = currentSpeed / maxSpeed; //Shift speed between 0 - 1 for curve evaluation.
        float steerFactor = steerignCurve.Evaluate(speedFraction) * steeringStrength; //Find the steer fact
        rigidBody.MoveRotation(rigidBody.rotation - steerFactor * Time.fixedDeltaTime * steeringInput);
    }

    public void SetThrottleInput(float throttle) {  throttleInput = throttle; }

    public void SetSteeringInput(float steering) { steeringInput = steering; }
}
