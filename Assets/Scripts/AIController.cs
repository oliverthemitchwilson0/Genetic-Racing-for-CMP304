using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class AIController : MonoBehaviour
{
    //Config
    public LayerMask trackLayer;
    public float rayDistance = 2f;
    private CarController carController;

    //Member
    public float[] genes { private get; set; }
    public bool isActive { get; private set; } = true;
    private float[] rayAngles = new float[]{ 0f, 36f, 72f, -36f, -72f };
    private float[] rayResults = new float[5];

    //Member - Tracking
    public float distanceTraveled { get; private set; }
    private Vector3 previousPosition;

    //Member - Stuck Detection
    private Vector3 lastCheckedPosition;
    private float stuckTimer = 0f;

    public void Destroy()
    {
        DestroyImmediate(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        carController = GetComponent<CarController>();
        previousPosition = transform.position;
        lastCheckedPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;
        if(genes.Length == 0) return;

        carController.SetThrottleInput(1f);
        carController.SetSteeringInput(0f);

        //AI Process
        TrackData();
        ProcessRayInput();
        ProcessDescision();

        //Stuck Detection
        IsStuck();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SetActive(false);
    }

    private void ProcessRayInput() 
    {
        Vector3 rayStartPosition = transform.position;
        Vector3 objDirection = transform.up;
        for (int i = 0; i < rayAngles.Length; i++)
        {
            Vector3 rayDirection = Quaternion.Euler(0f, 0f, rayAngles[i]) * objDirection;
            RaycastHit2D hit = Physics2D.Raycast(rayStartPosition, rayDirection, rayDistance, trackLayer);
            rayResults[i] = hit.fraction;

            //Debug.Log(hit.fraction);

            Debug.DrawRay(rayStartPosition, rayDirection * rayDistance, hit ? Color.green : Color.red);
        }
    }

    private void ProcessDescision()
    {
        float lowestResult = float.MaxValue;
        int priorityIndex = 0;
        for(int i = 0; i < rayResults.Length; i++) 
        {
            if (rayResults[i] < lowestResult)
            {
                lowestResult = rayResults[i];
                priorityIndex = i;
            }
        }

        int index = priorityIndex * 2;
        carController.SetThrottleInput(genes[index]);
        carController.SetSteeringInput(genes[index + 1]);
    }

    private void TrackData() 
    {
        //Track distance traveled.
        Vector3 currentPosiiton = transform.position;

        //if(currentPosiiton == previousPosition) isActive = false;

        distanceTraveled += Vector3.Distance(currentPosiiton, previousPosition);
        previousPosition = currentPosiiton;
    }

    private void IsStuck()
    {
        stuckTimer += Time.deltaTime;

        if(stuckTimer > 2f) 
        {
            Vector3 currentPosiiton = transform.position;
            float distance =  Vector3.Distance(currentPosiiton, lastCheckedPosition);
            if (distance < 0.1f) SetActive(false);
            lastCheckedPosition = currentPosiiton;
            stuckTimer -= 2f;
        }
    }

    private void SetActive(bool active) 
    {
        isActive = active;
        carController.SetThrottleInput(0f);
        carController.SetSteeringInput(0f);
    }
}
