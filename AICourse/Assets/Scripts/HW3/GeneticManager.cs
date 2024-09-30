using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticManager : MonoBehaviour
{
    public List<GeneticAgent>  agents= new List<GeneticAgent>();
    public int numberOfGenerations = 10;
    [SerializeField] int currentGeneration;
    [SerializeField] float _generationEvolveTimer;
    float currentTimer;

    private void Update()
    {
        if(ShouldEvolve())
        {
            if(currentGeneration<numberOfGenerations)
            {
                EvaluteFitness();
                EvolvePopulation();
                Respawn();
                currentGeneration++;
            }
        }
    }

    void EvolvePopulation()
    {
        float[] weights = agents[0].Weights;
        float highestFitness = agents[0].totalFitness;
        foreach (GeneticAgent agent in agents)
        {
            if(agent.totalFitness > highestFitness)
            {
                weights = agent.Weights;
                agent.totalFitness = highestFitness;
            }
        }

        foreach (GeneticAgent agent in agents)
        {
            agent.Weights = weights;
        }
    }

    void Respawn()
    {
        foreach (GeneticAgent agent in agents)
        {
            agent.Respawn();
        }
        EnvironmentManager.Instance.Respawn();
    }

    void EvaluteFitness()
    {
        foreach (var agent in agents)
        {
            agent.EvaluateFitness();
        }
    }

    bool ShouldEvolve()
    {
        currentTimer += Time.deltaTime;
        if(currentTimer> _generationEvolveTimer)
        {
            currentTimer = 0;
            return true;
        }
        return false;
    }
}
