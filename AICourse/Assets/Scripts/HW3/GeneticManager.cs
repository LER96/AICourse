using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticManager : MonoBehaviour
{
    public int pupolationCount;
    public GeneticAgent agent;
    public int numberOfGenerations = 10;
    [SerializeField] int currentGeneration;

    private void Update()
    {
        if(ShouldEvolve())
        {
            if(currentGeneration<numberOfGenerations)
            {
                EvaluteFitness();
                EvolvePopulation();
                currentGeneration++;
            }
        }
    }

    void EvolvePopulation()
    {

    }

    void EvaluteFitness()
    {
        agent.EvaluateFitness();
    }

    bool ShouldEvolve()
    {
        if (agent.health > 0 && agent.gameObject.activeInHierarchy)
        {
            return true;
        }
        return false;
    }
}
