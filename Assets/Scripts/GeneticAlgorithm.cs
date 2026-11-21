using System;
using UnityEngine;

//Come back to this later: https://stackoverflow.com/questions/14622342/elitism-in-ga-should-i-let-the-elites-be-selected-as-parents
public class GeneticAlgorithm<T>
{
    //Config
    private Func<T> geneGenerator;
    private Func<int, int> getFitnessData;
    private int maxPopulationSize { get; set; }
    private int individualGeneCount { get; set; }
    private int transferRatio { get; set; }
    private int crossoverThresholdRatio { get; set; }
    private int mutateThreshold {  get; set; }

    //Members
    public int generation { get; set; }
    public Individual<T>[] population { get; private set; }

    public GeneticAlgorithm(int populationSize, int geneCount, Func<T> geneGenerator, Func<int, int> getFitnessData, int transferRatio, int crossoverThresholdRatio, int mutateThreshold)
    {
        maxPopulationSize = populationSize;
        individualGeneCount = geneCount;
        this.geneGenerator = geneGenerator;
        this.getFitnessData = getFitnessData;
        this.transferRatio = transferRatio;
        this.crossoverThresholdRatio = crossoverThresholdRatio;
        this.mutateThreshold = mutateThreshold;
        generation = 1;

        //Generate initial population
        population = new Individual<T>[maxPopulationSize];

        for (int i = 0; i < maxPopulationSize; i++)
        {
            population[i] = new Individual<T>(individualGeneCount, geneGenerator, getFitnessData);
        }
    }

    public void Process() 
    {
        EvaluatePopulationFitness();
        CreateNextGeneration();
    }

    private void EvaluatePopulationFitness()
    {
        //Evaluate each fitness
        for (int i = 0; i < maxPopulationSize; i++)
        {
            population[i].EvaluateFitness(i);
        }

        //Sort in decending order.
        Array.Sort(population, (a, b) => {
            return b.fitness.CompareTo(a.fitness);
        });
    }

    private void CreateNextGeneration() 
    {
        //Create array of the next generation.
        Individual<T>[] nextGeneration = new Individual<T>[maxPopulationSize];

        //Transfer percentage of top individuals to the next generation (Eliptism).
        int transferCount = maxPopulationSize * transferRatio / 100;
        Array.Copy(population, 0, nextGeneration, 0, transferCount);

        //Begin crossover between a percentage of the previous generation for the rest of the next generation.
            //Mutate child
        int crossoverThreshold = maxPopulationSize * crossoverThresholdRatio / 100;
        for(int i = transferCount; i < maxPopulationSize; i++) 
        {
            //Get references to two random parents.
            ref Individual<T> a = ref population[UnityEngine.Random.Range(0, crossoverThreshold)];
            ref Individual<T> b = ref population[UnityEngine.Random.Range(0, crossoverThreshold)];

            while(b == a)
                b = ref population[UnityEngine.Random.Range(0, crossoverThreshold)];

            Individual<T> child = SinglePointCrossover(ref a, ref b);
            Mutate(ref child);

            nextGeneration[i] = child;
        }

        //Set population to the enxt generation.
        population = nextGeneration;
        generation++;
        Debug.Log(generation);
    }

    //Classes in C# are automatically passed by reference 
    private Individual<T> SinglePointCrossover(ref Individual<T> a, ref Individual<T>b) 
    {
        //Select random cut point oof gene length.
        int geneLength = a.genes.Length;
        int cutPoint = UnityEngine.Random.Range(1, geneLength);

        //Create new gene array.
        T[] crossedGenes = new T[geneLength];

        //Crossover genes of parent a and b using cutoff point.
        Array.Copy(a.genes, 0, crossedGenes, 0, cutPoint);
        Array.Copy(b.genes, cutPoint, crossedGenes, cutPoint, geneLength - cutPoint);

        //Return new Individual with gene array.
        return new Individual<T>(in crossedGenes, getFitnessData);
    }

    private void Mutate(ref Individual<T> x) 
    {
        for (int i = 0; i < x.genes.Length; i++)
        {
            if(UnityEngine.Random.Range(0, 100) < mutateThreshold) 
            {
                x.genes[i] = geneGenerator();
            }
        }
    }
}
