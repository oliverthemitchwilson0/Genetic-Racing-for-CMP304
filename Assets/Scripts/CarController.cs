using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    public float acceleration = 3.0f;
    public float maxSpeed = 10.0f;
    public InputAction steering;
    public InputAction throttle;

    private Rigidbody2D rb;
    public float speed = 0.0f;

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
        rb.linearVelocity = transform.up * speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.AngleAxis(
            120.0f * steering.ReadValue<float>() * Time.deltaTime,
            Vector3.back
        );

        speed += (acceleration * Time.deltaTime) * throttle.ReadValue<float>();
        speed = Mathf.Clamp( speed, -maxSpeed / 2, maxSpeed );
    }
}
