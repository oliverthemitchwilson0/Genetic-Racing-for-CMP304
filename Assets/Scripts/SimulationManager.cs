using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SimulationManager : MonoBehaviour
{
    //Config
    [Header("Simulation Config")]
    public float simulationSpeed = 1f;
    public GameObject carPrefab;
    public int simulationCount = 30;
    public int geneCount = 12;
    public int transferRatio = 10;
    public int tournamentSize = 30;
    public int mutateThreshold = 5;
    public int mutationRatio = 25;

    //Config - UI
    [Header("UI")]
    public Slider simSpeedSlider;
    public bool logFitness;

    //Members
    private bool simulationInProgress { get; set; } = false;
    private GeneticAlgorithm<float> algorithm;
    private GameObject[] objects;
    private AIController[] controllers;
    private AIFitness[] aiFitness;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSimulation();
    }

    public void BeginSimulation()
    {
        if (simulationInProgress) return;
        algorithm = new GeneticAlgorithm<float>(simulationCount, geneCount, GeneGenerator, GeneMutator, transferRatio, tournamentSize, mutateThreshold);
        CreateNewSimulationObjects();
        simulationInProgress = true;
    }

    public void UpdateSimulation() 
    {
        if (!simulationInProgress) return;
        if (simulationCount == 0) return;

        Time.timeScale = simSpeedSlider.value;

        int inactiveCounter = 0;
        foreach (GameObject car in objects)
        {
            if (!car.GetComponent<AIController>().isActive)
                inactiveCounter++;
        }

        if (inactiveCounter == simulationCount)
        {
            //Assign each Induvidual's fitness with the evaluated fitness.
            for (int i = 0; i < simulationCount; i++)
            {
                algorithm.population[i].fitness = aiFitness[i].EvaluateFitness();
            }

            if(logFitness)
                LogGenerationFitness();

            algorithm.Process();

            //Reset sim
            for (int i = 0; i < simulationCount; i++)
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

    float GeneMutator(float gene) 
    {
        float mutation = mutationRatio / 100f;
        return Math.Clamp(gene += UnityEngine.Random.Range(-mutation, mutation), -1f, 1f);
    }

    private void LogGenerationFitness() 
    {
        string path = Application.persistentDataPath + "/FitnessLog.txt";

        string[] content =
        {
            "Generation: " + algorithm.generation.ToString(),
            "   Best Fitness: " + algorithm.GetBestFitness().ToString(),
            "   Average Fitness: " + algorithm.GetAverageFitness().ToString(),
        };

        File.AppendAllLines(path, content);

        //Debug.Log(path);
    }

    public void OnLogFitnessToggle(bool value) 
    {
        logFitness = value;
    }
}
