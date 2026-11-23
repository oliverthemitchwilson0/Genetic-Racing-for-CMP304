using UnityEngine;

public class AIFitness : MonoBehaviour
{
    //Config
    [Header("Weighting Values")]
    //public int distanceWeight;
    public int checkpointWeight;
    //public int lapsWeight;
    //public int speedWeight;

    //Members
    private int checkpointsReached { get; set; } = 0;
    private int nextCheckpoint { get; set; } = 0;

    public void CheckpointReached(int checkpointIndex) 
    {
        if(checkpointIndex == nextCheckpoint) 
        {
            checkpointsReached++;
            nextCheckpoint++;
        }
        //else 
        //{
        //    GetComponent<AIController>().SetActive(false);
        //}
    }

    public int EvaluateFitness() 
    {
        int fitness = checkpointsReached;
        return fitness;
    }
}
