using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public GameObject carPrefab;
    public int simulationCount = 30;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < simulationCount; i++)
        {
            Instantiate(carPrefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
