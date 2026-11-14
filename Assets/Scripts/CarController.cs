using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

//Car Controlling Adapted from: https://www.youtube.com/watch?v=d7IY7mJECRk
public class CarController : MonoBehaviour
{
    public InputAction steering;
    public InputAction throttle;
    public float acceleration = 3.0f;
    public float friction = 1f;
    public float maxDriveSpeed = 10.0f;
    public float steerStrength = 120.0f;
    public AnimationCurve steeringBySpeed;
    public LayerMask trackLayer;

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
        float throttleInput = throttle.ReadValue<float>();
        if (throttleInput > 0.0f)
            speed += (acceleration * Time.fixedDeltaTime);
        else
            speed -= friction * Time.fixedDeltaTime;

        speed = Mathf.Clamp(speed, 0f, maxDriveSpeed);
        rb.linearVelocity = transform.up * speed;

        float speedFraction = speed / maxDriveSpeed;
        float steerFactor = steeringBySpeed.Evaluate(speedFraction) * steerStrength;
        float steerInput = steering.ReadValue<float>();
        rb.MoveRotation(rb.rotation - steerFactor * Time.fixedDeltaTime * steerInput);

        //Raycast test

        RaycastHit2D hit = Physics2D.Raycast(rb.position, transform.up, 2.0f, trackLayer);
        if(hit.collider != null && throttleInput == 0.0f)
        {
            Debug.Log("Hit");
            //speed -= (acceleration * Time.fixedDeltaTime);
            //speed = Mathf.Clamp(speed, 0.0f, maxDriveSpeed);
        }
    }
    private void Update()
    {
        Debug.DrawRay(transform.position, transform.up * 2.0f, Color.red);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        speed = 0f;

        Vector3 newPosition = new Vector3(0f, -3.6f, 0f);
        gameObject.transform.position = newPosition;

        Quaternion newRotation = Quaternion.Euler(0, 0, 90f);
        gameObject.transform.rotation = newRotation;
    }
}
