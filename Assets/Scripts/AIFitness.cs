using UnityEngine;

public class AIFitness : MonoBehaviour
{
    //Config
    [Header("Weighting Values")]
    public int distanceWeight = 1;
    public int checkpointWeight = 100;
    public int lapsWeight = 500;
    public int speedWeight = 50;

    //Members
    private int checkpointsReached { get; set; } = 0;
    private int lapsFinished { get; set; } = 0;
    private int nextCheckpoint { get; set; } = 0;
    private float timeAlive { get; set; } = 0f;

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

    public void CheckpointReached(int checkpointIndex, bool finishLine) 
    {
        //Early exit if controller is inactive.
        AIController controller = GetComponent<AIController>();
        if (!controller.isActive) return;

        if(checkpointIndex == nextCheckpoint) 
        {
            checkpointsReached++;
            nextCheckpoint++;

            if (finishLine)
            {
                lapsFinished++;
                nextCheckpoint = 0;

                if(lapsFinished == 3) controller.SetActive(false);
            }
        }
        else
            controller.SetActive(false);
    }

    //public void FinishLineReached(int checkpoint) 
    //{
    //    if (checkpoint == nextCheckpoint)
    //    {
    //        checkpointsReached++;
    //        nextCheckpoint++;
    //    }
    //}

    public int EvaluateFitness() 
    {
        int fitness = 0;

        float averageSpeed = distanceTraveled / timeAlive;

        fitness += (int)distanceTraveled * distanceWeight;
        fitness += checkpointsReached * checkpointWeight;
        fitness += lapsFinished * lapsWeight;

        if (checkpointsReached > 0) fitness += (int)averageSpeed * speedWeight;

        return fitness;
    }

    private void TrackDistance() 
    {
        //Early exit if controller is inactive.
        if (!GetComponent<AIController>().isActive) return;

        //Track Time
        timeAlive += Time.deltaTime;

        //Track distance traveled.
        Vector3 currentPosiiton = transform.position;
        distanceTraveled += Vector3.Distance(currentPosiiton, previousPosition);
        previousPosition = currentPosiiton;
    }
}
