using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int index = 0;
    public bool finishLine = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Car")) 
        {
            AIFitness aiFitness = collision.gameObject.GetComponent<AIFitness>();
            aiFitness.CheckpointReached(index);
        }
    }
}
