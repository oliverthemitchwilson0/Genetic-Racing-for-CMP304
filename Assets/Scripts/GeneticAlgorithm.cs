using System;
using UnityEngine;

//Come back to this later: https://stackoverflow.com/questions/14622342/elitism-in-ga-should-i-let-the-elites-be-selected-as-parents
//https://www.geeksforgeeks.org/dsa/genetic-algorithms/
public class GeneticAlgorithm<T>
{
    //Config
    private Func<T> geneGenerator;
    private Func<T, T> geneMutator;
    private int maxPopulationSize { get; set; }
    private int individualGeneCount { get; set; }
    private int transferRatio { get; set; }
    private int tournamentSize { get; set; }
    private int mutateThreshold { get; set; }

    //Members
    public int generation { get; set; }
    public Individual<T>[] population { get; private set; }

    public GeneticAlgorithm(int populationSize, int geneCount, Func<T> geneGenerator, Func<T, T> geneMutator, int transferRatio, int tournamentSize, int mutateThreshold)
    {
        maxPopulationSize = populationSize;
        individualGeneCount = geneCount;
        this.geneGenerator = geneGenerator;
        this.geneMutator = geneMutator;
        this.transferRatio = transferRatio;
        this.tournamentSize = tournamentSize;
        this.mutateThreshold = mutateThreshold;
        generation = 1;

        //Generate initial population
        population = new Individual<T>[maxPopulationSize];

        for (int i = 0; i < maxPopulationSize; i++)
        {
            population[i] = new Individual<T>(individualGeneCount, geneGenerator);
        }
    }

    public void Process()
    {
        SortPopulation();
        CreateNextGeneration();
    }

    private void SortPopulation()
    {
        //Sort in decending order.
        Array.Sort(population, (a, b) => {
            return b.fitness.CompareTo(a.fitness);
        });

        ////Test
        //for (int i = 0; i < population.Length; i++)
        //{
        //    string str = i.ToString() + ": " + population[i].fitness.ToString();
        //    Debug.Log(str);
        //}
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
        for (int i = transferCount; i < maxPopulationSize; i++)
        {
            //Reference to parents
            Individual<T> a;
            Individual<T> b;

            ParentSelection(out a, out b);

            Individual<T> child = SinglePointCrossover(a, b);
            Mutate(child);

            nextGeneration[i] = child;
        }

        //Set population to the enxt generation.
        population = nextGeneration;
        generation++;

        string str = "Generation: " + generation.ToString();
        Debug.Log(str);
    }

    private void ParentSelection(out Individual<T> a, out Individual<T> b)
    {
        a = TournamentSelection(tournamentSize);
        b = TournamentSelection(tournamentSize);

        while (b == a)
            b = TournamentSelection(tournamentSize);
    }

    private Individual<T> RandomSelection()
    {
        int crossoverThreshold = maxPopulationSize * tournamentSize / 100;
        return population[UnityEngine.Random.Range(0, crossoverThreshold)];
    }

    private Individual<T> TournamentSelection(int tournamentSize = 3)
    {
        int bestIndex = UnityEngine.Random.Range(0, maxPopulationSize);

        for (int i = 1; i < tournamentSize; i++)
        {
            int competitorIndex = UnityEngine.Random.Range(0, maxPopulationSize);
            if (population[competitorIndex].fitness > population[bestIndex].fitness) bestIndex = competitorIndex;
        }

        return population[bestIndex];
    }

    //Classes in C# are automatically passed by reference 
    private Individual<T> SinglePointCrossover(Individual<T> a, Individual<T> b)
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
        return new Individual<T>(in crossedGenes);
    }

    private void Mutate(Individual<T> x)
    {
        for (int i = 0; i < x.genes.Length; i++)
        {
            if (UnityEngine.Random.Range(0, 100) < mutateThreshold)
            {
                x.genes[i] = geneMutator(x.genes[i]);
            }
        }
    }

    public int GetBestFitness()
    {
        int bestFitness = int.MinValue;
        for (int i = 0; i < population.Length; i++) 
        {
            if (population[i].fitness > bestFitness) 
            {
                bestFitness = population[i].fitness;
            }
        }

        return bestFitness;
    }

    public float GetAverageFitness() 
    {
        float totalFitness = 0;
        for (int i = 0; i < population.Length; i++)
            totalFitness += population[i].fitness;

        return totalFitness / maxPopulationSize;    
    }
}
