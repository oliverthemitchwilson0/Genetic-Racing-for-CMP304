using UnityEngine;

public class AIFitness : MonoBehaviour
{
    //Config
    [Header("Weighting Values")]
    public int checkpointWeight;
    public int fastTimeWeight;

    //Members
    private int checkpointsReached { get; set; } = 0;
    private int nextCheckpoint { get; set; } = 0;
    private int lapsCompleted { get; set; } = 0;
    private float timeAlive { get; set; } = 0;

    private void FixedUpdate()
    {
        if (!IsActive()) return;
        TrackData();
        if(lapsCompleted >= 3) GetComponent<AIController>().SetActive(false);
    }

    public void SignalCheckpointReached(int checkpointIndex, bool finishLine) 
    {
        if (!IsActive()) return;

        if(checkpointIndex == nextCheckpoint) 
        {
            checkpointsReached++;
            nextCheckpoint++;

            //Finish Line Logic
            if (finishLine) 
            {
                lapsCompleted++;
                nextCheckpoint = 0;
            }
        }
    }

    public int EvaluateFitness() 
    {
        int fitness = 0;
        fitness += checkpointsReached * checkpointWeight;
        fitness += (lapsCompleted >= 3) ? Mathf.RoundToInt(fastTimeWeight / timeAlive) : 0;
        return fitness;
    }

    private bool IsActive()
    {
        return GetComponent<AIController>().isActive;
    }

    private void TrackData() 
    {
        timeAlive += Time.fixedDeltaTime;
    }

    public float FinishTime() 
    {
        if(lapsCompleted >= 3) 
        {
            return timeAlive;
        }
        return 0f;
    }
}
