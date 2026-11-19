using System;
using UnityEngine;

public class Individual<T>
{
    public T[] genes { get; private set; }
    public int fitness { get; private set; }

    //private System.Random random;

    public Individual(int size, Func<T> geneGenerator)
    {
        //Assign new gene array
        genes = new T[size];
        fitness = 0;

        //Populate genes with custom random func.
        for (int i = 0; i < size; i++)
        {
            genes[i] = geneGenerator();
        }
    }

    public Individual(in T[] genesIn) 
    {
        genes = genesIn;
        fitness = 0;
    }

    public void EvaluateFitness() 
    {
        //Reset Fitness

        //Evaluate Fitness
    }
}
