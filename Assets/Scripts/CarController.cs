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

    public bool debugInput = false;
    private float currentSpeed = 0f;
    private bool isActive = true;

    public InputAction throttleDebugInput;
    public InputAction steeringInputAction;
    private float steeringInput = 0f;
    private float throttleInput = 0f;

    private void OnEnable()
    {
        throttleDebugInput.Enable();
        steeringInputAction.Enable();
    }

    private void OnDisable()
    {
        throttleDebugInput.Disable();
        steeringInputAction.Disable();
    }

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
        float input = 0f;
        if (debugInput)
            input = throttleDebugInput.ReadValue<float>();
        else
            input = throttleInput;

        if (input != 0.0f)   //Throttle Positive: Accelerate.
            currentSpeed += ((acceleration * input) * Time.fixedDeltaTime);
        else                        //Throttle anything else: apply friction.
            currentSpeed -= friction * Time.fixedDeltaTime;

        //Possibly add braking for negative throttleInput.

        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
        rigidBody.linearVelocity = transform.up * currentSpeed;
    }

    private void Steering()
    {
        float input = 0f;
        if(debugInput)
            input = steeringInputAction.ReadValue<float>();
        else
            input = steeringInput;

        float speedFraction = currentSpeed / maxSpeed; //Shift speed between 0 - 1 for curve evaluation.
        float steerFactor = steerignCurve.Evaluate(speedFraction) * steeringStrength; //Find the steer fact
        rigidBody.MoveRotation(rigidBody.rotation - (steerFactor * input) * Time.fixedDeltaTime);
    }

    public void SetThrottleInput(float throttle) {  throttleInput = throttle; }

    public void SetSteeringInput(float steering) { steeringInput = steering; }
}
