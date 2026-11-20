using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    //Config
    public float simulationSpeed = 1f;
    public GameObject carPrefab;
    public int simulationCount = 30;
    private int geneCount = 10;

    //Members
    private GeneticAlgorithm<float> algorithm;
    private AIController[] controllers;
    private bool simulationInProgress = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        algorithm = new GeneticAlgorithm<float>(simulationCount, geneCount, GeneGenerator, RetrieveFitnessData);
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
        Time.timeScale = simulationSpeed;

        int unactiveCounter = 0;
        foreach (AIController controller in controllers)
        {
            if(!controller.isActive)
                unactiveCounter++;
        }

        if(unactiveCounter == simulationCount) 
        {
            algorithm.Process();

            //Reset sim
            for(int i = 0; i < simulationCount; i++) 
            {
                controllers[i].Destroy();
            }

            controllers = new AIController[simulationCount];

            for (int i = 0; i < simulationCount; i++)
            {
                GameObject gameObject = Instantiate(carPrefab);
                controllers[i] = gameObject.GetComponent<AIController>();
                controllers[i].genes = algorithm.population[i].genes;
            }
        }
    }

    float GeneGenerator()
    {
        return UnityEngine.Random.Range(-1f, 1f);
    }

    int RetrieveFitnessData(int index) 
    {
        return (int)controllers[index].distanceTraveled;
    }
}
