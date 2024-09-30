using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticManager : MonoBehaviour
{
    public List<GeneticAgent>  agents= new List<GeneticAgent>();
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
        //agents.EvaluateFitness();
    }

    bool ShouldEvolve()
    {
        foreach (var agent in agents)
        {
            if (agent.health > 0 && agent.gameObject.activeInHierarchy)
            {
                return false;
            }
        }
        return true;
    }
}
