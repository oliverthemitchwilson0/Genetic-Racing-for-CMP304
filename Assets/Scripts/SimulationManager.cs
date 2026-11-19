using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    //Config
    public GameObject carPrefab;
    public int simulationCount = 30;

    //Members
    private GeneticAlgorithm<float> algorithm;
    private AIController[] controllers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        algorithm = new GeneticAlgorithm<float>(simulationCount, 3, GeneGenerator);
        controllers = new AIController[simulationCount];

        for (int i = 0; i < simulationCount; i++)
        {
            GameObject gameObject = Instantiate(carPrefab);
            controllers[i] = gameObject.GetComponent<AIController>();
            controllers[i].genes = algorithm.population[i].genes;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float GeneGenerator()
    {
        return UnityEngine.Random.Range(-1f, 1f);
    }
}
