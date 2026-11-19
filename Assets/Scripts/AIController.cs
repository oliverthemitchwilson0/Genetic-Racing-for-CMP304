using System.Collections.Generic;
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
    private List<float> rayAngles = new List<float> { 0f, 25f, -25f };
    private List<bool> rayResults = new List<bool> { false, false, false };

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

        //Track Distance.
        Vector3 currentPosiiton = transform.position;
        distanceTraveled += Vector3.Distance(currentPosiiton, previousPosition);
        previousPosition = currentPosiiton;

        //Ray Loop
        Vector3 startPosition = transform.position;
        Vector3 startDirection = transform.up;
        for(int i = 0; i < rayAngles.Count; i++) 
        {
            Vector3 rayDirection = Quaternion.Euler(0f, 0f, rayAngles[i]) * startDirection;
            RaycastHit2D hit = Physics2D.Raycast(startPosition, rayDirection, rayDistance, trackLayer);
            if (hit.collider != null)
            {
                rayResults[i] = true;
            }
            else
                rayResults[i] = false;

            Debug.DrawRay(startPosition, rayDirection * rayDistance, Color.red);
        }

        //Temp Inputs
        if (rayResults[0] == true)
            carController.SetThrottleInput(genes[0]);

        if (rayResults[1] == true)
            carController.SetSteeringInput(genes[1]);

        if (rayResults[2] == true)
            carController.SetSteeringInput(genes[2]);
    }

    private void OnDestroy()
    {
        Debug.Log(distanceTraveled);
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
