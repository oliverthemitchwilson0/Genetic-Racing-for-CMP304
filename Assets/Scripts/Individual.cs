using System;
using UnityEngine;

public class Individual<T>
{
    //Config
    private Func<int, int> getFitnessData;

    //Members
    public T[] genes { get; private set; }
    public int fitness { get; private set; }

    //private System.Random random;

    public Individual(int size, Func<T> geneGenerator, Func<int, int> getFitnessData)
    {
        //Assign new gene array
        genes = new T[size];
        fitness = 0;

        this.getFitnessData = getFitnessData;

        //Populate genes with custom random func.
        for (int i = 0; i < size; i++)
        {
            genes[i] = geneGenerator();
        }
    }

    public Individual(in T[] genesIn, Func<int, int> getFitnessData) 
    {
        genes = genesIn;
        fitness = 0;

        this.getFitnessData = getFitnessData;
    }

    public void EvaluateFitness(int index) 
    {
        //Reset Fitness
        fitness = 0;

        //Evaluate Fitness
        fitness = getFitnessData(index);
    }
}
