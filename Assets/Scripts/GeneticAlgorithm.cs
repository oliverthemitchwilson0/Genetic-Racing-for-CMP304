using System;
using UnityEngine;

public class GeneticAlgorithm<T>
{
    private Individual<T>[] population { set; get; }
    private int geneCount = 3;

    public GeneticAlgorithm(int populationSize, Func<T> randomFunc)
    {
        population = new Individual<T>[populationSize];

        for (int i = 0; i < populationSize; i++)
        {
            population[i] = new Individual<T>(geneCount, randomFunc);
        }
    }
}
