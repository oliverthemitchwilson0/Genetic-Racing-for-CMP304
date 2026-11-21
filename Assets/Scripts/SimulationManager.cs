using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    //Config
    public float simulationSpeed = 1f;
    public GameObject carPrefab;
    public int simulationCount = 30;
    public int transferRatio = 10;
    public int crossoverThresholdRatio = 30;
    public int mutateThreshold = 5;
    private int geneCount = 10;

    //Members
    private GeneticAlgorithm<float> algorithm;
    private GameObject[] objects;
    private AIController[] controllers;
    private AIFitness[] aiFitness;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        algorithm = new GeneticAlgorithm<float>(simulationCount, geneCount, GeneGenerator, transferRatio, crossoverThresholdRatio, mutateThreshold);
        CreateNewSimulationObjects();
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = simulationSpeed;

        int unactiveCounter = 0;
        foreach (GameObject car in objects)
        {
            if(!car.GetComponent<AIController>().isActive)
                unactiveCounter++;
        }

        if(unactiveCounter == simulationCount) 
        {
            //Assign each Induvidual's fitness with the evaluated fitness.
            for(int i = 0; i < simulationCount; i++) 
            {
                algorithm.population[i].fitness = aiFitness[i].EvaluateFitness();
            }

            algorithm.Process();

            //Reset sim
            for(int i = 0; i < simulationCount; i++) 
            {
                DestroyImmediate(objects[i]);
            }

            CreateNewSimulationObjects();
        }
    }

    private void CreateNewSimulationObjects() 
    {
        objects = new GameObject[simulationCount];
        controllers = new AIController[simulationCount];
        aiFitness = new AIFitness[simulationCount];

        for (int i = 0; i < simulationCount; i++)
        {
            objects[i] = Instantiate(carPrefab, transform.position, transform.rotation);
            controllers[i] = objects[i].GetComponent<AIController>();
            aiFitness[i] = objects[i].GetComponent<AIFitness>();
            controllers[i].genes = algorithm.population[i].genes;
        }
    }

    float GeneGenerator()
    {
        return UnityEngine.Random.Range(-1f, 1f);
    }
}
