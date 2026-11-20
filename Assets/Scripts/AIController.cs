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
    private bool[] rayResults = new bool[]{ false, false, false, false, false };

    //Member - Tracking
    public float distanceTraveled { get; private set; }
    private Vector3 previousPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        carController = GetComponent<CarController>();
        previousPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;
        if(genes.Length == 0) return;

        carController.SetThrottleInput(1f);
        carController.SetSteeringInput(0f);

        TrackData();
        ProcessRayInput();

        //Temp Inputs
        //if (rayResults[0] == true)
        //    carController.SetThrottleInput(genes[0]);

        //if (rayResults[1] == true)
        //    carController.SetSteeringInput(genes[1]);

        //if (rayResults[2] == true)
        //    carController.SetSteeringInput(genes[2]);
    }

    private void ProcessRayInput() 
    {
        Vector3 rayStartPosition = transform.position;
        Vector3 objDirection = transform.up;
        for (int i = 0; i < rayAngles.Length; i++)
        {
            Vector3 rayDirection = Quaternion.Euler(0f, 0f, rayAngles[i]) * objDirection;
            RaycastHit2D hit = Physics2D.Raycast(rayStartPosition, rayDirection, rayDistance, trackLayer);
            if (hit.collider != null)
            {
                rayResults[i] = true;
            }
            else
                rayResults[i] = false;

            Debug.DrawRay(rayStartPosition, rayDirection * rayDistance, rayResults[i] ? Color.green : Color.red);
        }
    }

    private void TrackData() 
    {
        //Track distance traveled.
        Vector3 currentPosiiton = transform.position;
        distanceTraveled += Vector3.Distance(currentPosiiton, previousPosition);
        previousPosition = currentPosiiton;
    }

    private void OnDestroy()
    {
        
    }

    public void Destroy() 
    {
        DestroyImmediate(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isActive = false;
    }
}
