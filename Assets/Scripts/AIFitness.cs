using UnityEngine;

public class AIFitness : MonoBehaviour
{
    //Members
    private int fitness { get; set; } = 0;
    private int checkpointsReached { get; set; } = 0;
    private int nextCheckpoint { get; set; } = 0;

    //Member - Distance Tracking
    public float distanceTraveled { get; private set; }
    private Vector3 previousPosition;

    private void Start()
    {
        previousPosition = transform.position;
    }

    private void Update()
    {
        TrackDistance();
    }

    public void CheckpointReached(int checkpoint) 
    {
        if(checkpoint == nextCheckpoint) 
        {
            checkpointsReached++;
            nextCheckpoint++;
        }
    }

    public int EvaluateFitness() 
    {
        fitness += (int)distanceTraveled;
        fitness += checkpointsReached * 100;
        return fitness;
    }

    private void TrackDistance() 
    {
        //Track distance traveled.
        Vector3 currentPosiiton = transform.position;
        distanceTraveled += Vector3.Distance(currentPosiiton, previousPosition);
        previousPosition = currentPosiiton;
    }
}
