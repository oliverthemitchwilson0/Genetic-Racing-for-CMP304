using System;
using UnityEngine;

public class Individual<T>
{
    public T[] genes { get; private set; }

    private System.Random random;

    public Individual(int size, Func<T> randomFunc)
    {
        //Assign new gene array
        genes = new T[size];

        //Populate genes with custom random func.
        for (int i = 0; i < size; i++)
        {
            genes[i] = randomFunc();
        }
    }
}
