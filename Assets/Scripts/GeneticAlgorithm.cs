using System;
using UnityEngine;

//Come back to this later: https://stackoverflow.com/questions/14622342/elitism-in-ga-should-i-let-the-elites-be-selected-as-parents
public class GeneticAlgorithm<T>
{
    //Config
    private int maxPopulationSize { get; set; }
    private int individualGeneCount { get; set; }
    private int transferRatio { get; set; }
    private int crossoverThresholdRatio { get; set; }
    private int mutateThreshold {  get; set; }

    //Members
    private Individual<T>[] population { get; set; }

    public GeneticAlgorithm(int populationSize, int geneCount, Func<T> randomFunc)
    {
        maxPopulationSize = populationSize;
        individualGeneCount = geneCount;
        transferRatio = 10;
        crossoverThresholdRatio = 30;
        mutateThreshold = 5;

        //Generate initial population
        population = new Individual<T>[maxPopulationSize];

        for (int i = 0; i < maxPopulationSize; i++)
        {
            population[i] = new Individual<T>(individualGeneCount, randomFunc);
        }
    }

    public void EvaluatePopulationFitness()
    {
        //Evaluate each fitness
        for (int i = 0; i < maxPopulationSize; i++)
        {
            population[i].EvaluateFitness();
        }

        //Sort in decending order.
        Array.Sort(population, (a, b) => {
            return b.fitness.CompareTo(a.fitness);
        });
    }

    public void GenerateNextGeneration() 
    {
        //Create array of the next generation.
        Individual<T>[] nextGeneration = new Individual<T>[maxPopulationSize];

        //Transfer percentage of top individuals to the next generation (Eliptism).

        //Begin crossover between a percentage of the previous generation for the rest of the next generation.
            //Mutate child

        //Set population to the enxt generation.
        population = nextGeneration;
    }

    private void SinglePointCrossover(ref Individual<T> a, ref Individual<T>b) 
    {
        //Select random cut point oof gene length.

        //Create new gene array.

        //Crossover genes of parent a and b using cutoff point.

        //Call Mutate on newly greated gene array.
        
        //Return new Individual with gene array.
    }

    private void Mutate(ref float[] x) 
    {
        //For each gene in array
            //Weigh chance with mutateThreshold. if true:
                //Overwrite gene with random gene.
    }
}
