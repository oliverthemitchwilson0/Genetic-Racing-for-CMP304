using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    //Simulation Config
    public GameObject carPrefab;
    public int simulationCount = 30;

    private GeneticAlgorithm<float> algorithm;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        algorithm = new GeneticAlgorithm<float>(simulationCount, GetRandom);

        for (int i = 0; i < simulationCount; i++)
        {
            Instantiate(carPrefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float GetRandom()
    {
        return UnityEngine.Random.Range(-1f, 1f);
    }
}
