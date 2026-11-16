using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public LayerMask trackLayer;
    public float rayDistance = 2f;

    CarController carController;

    List<float> rayAngles = new List<float> { 0f, 25f, -25f };
    List<bool> rayResults = new List<bool> { false, false, false };

    //Test
    List<float> DNA = new List<float>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        carController = GetComponent<CarController>();

        //Test
        //Create a throttle and steering bias for each ray.
        for(int i = 0; i < 3; i++)
        {
            float throttleBias = UnityEngine.Random.Range(-1f, 1f);
            DNA.Add(throttleBias);

            float steeringBias = UnityEngine.Random.Range(-1f, 1f);
            DNA.Add(steeringBias);
        }
    }

    // Update is called once per frame
    void Update()
    {
        carController.SetThrottleInput(1f);
        carController.SetSteeringInput(0f);

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
            carController.SetThrottleInput(0f);

        if (rayResults[1] == true)
            carController.SetSteeringInput(1f);

        if (rayResults[2] == true)
            carController.SetSteeringInput(-1f);
    }
}
