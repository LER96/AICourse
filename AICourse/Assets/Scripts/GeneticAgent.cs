using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAgent : Character
{
    public float fitness;
    public bool detected;
    public GameObject parent;

    [SerializeField] float[] weights = new float[3];
    [Range(0,1)]
    [SerializeField] float decisionValue;
    float decision;

    [SerializeField] float healthprecentage;
    //Enemy ratio
    [Header("Near Enemy")]
    float _currentNearEnemyDistance;
    [SerializeField] float minEnemyDistance;
    [SerializeField] float maxEnemyDistance;
    //Near Pack Ratio
    [Header("Near Pack")]
    float _currentNearPackDistance;
    [SerializeField] float maxPackDistance;
    [SerializeField] float minPackDistance;

    [Header("States")]
    [SerializeField] StateSpeed patrol;
    [SerializeField] StateSpeed escape;

    float _maxHp;
    Transform nearbyPack;
    FuzzyLocig fuzzyLocig;

    public override void Start()
    {
        _maxHp = health;
        fuzzyLocig = new FuzzyLocig();
        InitWeight();
        OnStart();
    }
    public void OnStart()
    {
        health = _maxHp;
        SetPartolPoint();
        SetState(patrol);
    }

    void InitWeight()
    {
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = Random.Range(0f, 1f);
        }
    }

    private void Update()
    {
        EvaluateFozzy();
        ManageState();
    }

    public void EvaluateFitness()
    {
        fitness = 0;

        float survivalTime = Time.timeSinceLevelLoad;
        fitness += survivalTime;


    }

    public void EvaluateFozzy()
    {
        CheckCloseHealthPack();
        CheckCloseEnemy();
        float hpRatio= fuzzyLocig.CalculateHp(health, _maxHp);
        float distEnemyRatio= fuzzyLocig.CalculateNearEnemyDistRatio(minEnemyDistance, maxEnemyDistance, _currentNearEnemyDistance);
        float distPackRatio = fuzzyLocig.CalculateNearPackDistRatio(minPackDistance, maxPackDistance, _currentNearPackDistance);

        decision = fuzzyLocig.EvaluatePriotriy(distEnemyRatio, hpRatio, distPackRatio);
        if(decision>decisionValue)
        {
            SetState(escape);
        }
        else
        {
            SetState(patrol);
        }
    }

    void ManageState()
    {
        switch(_characterStateEnum)
        {
            case CharacterState.Patrol:
                Patrol();
                break;
            case CharacterState.Run:
                GoToNearPack();
                break;
        }
    }

    void CheckCloseHealthPack()
    {
        float nearbyPackDist = 6000;
        foreach (Transform pack in EnvironmentManager.Instance.HealthPacks)
        {
            nearbyPack = pack;
            float dist = Vector3.Distance(transform.position, pack.transform.position);
            if (dist < nearbyPackDist)
            {
                nearbyPack = pack;
                nearbyPackDist = dist;
            }
        }
        _currentNearPackDistance = nearbyPackDist;
    }

    void GoToNearPack()
    {
        _pathFindUnit.SetDestanation(nearbyPack.position);
    }

    void CheckCloseEnemy()
    {
        float nearbyEnemyDist = 6000;
        foreach (Transform pack in EnvironmentManager.Instance.HealthPacks)
        {
            float dist = Vector3.Distance(transform.position, pack.transform.position);
            if (dist < nearbyEnemyDist)
            {
                nearbyEnemyDist = dist;
            }
        }
        _currentNearEnemyDistance = nearbyEnemyDist;

    }

    public override void Patrol()
    {
        if(_pathFindUnit.reachedTarget)
            SetPartolPoint();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            EnemyBehavior enemy = other.GetComponent<EnemyBehavior>();
            if(enemy!=null)
            {
                TakeDamage(25);
            }
        }
        else if(other.CompareTag("Health"))
        {
            HealthPack pack = other.GetComponent<HealthPack>();
            if (pack != null)
            {
                health += pack.hp;
                fitness += 30;
            }
        }
    }

    public override void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            parent.SetActive(false);
        }
    }

}

public class FuzzyLocig
{
    public FuzzyLocig()
    {

    }

    public float EvaluatePriotriy(float enemyDistRatio, float agentHpRatio, float packDistRatio)
    {
        return (enemyDistRatio + agentHpRatio + packDistRatio) / 3;
    }

    public float CalculateHp(float hp, float max)
    {
        return hp / max;
    }

    public float CalculateNearEnemyDistRatio(float min, float max, float current)
    {
        if(current<min)
        {
            return 1;
        }
        else if(current>max)
        {
            return 0;
        }
        else
        {
            return 1 - (min / current);
        }
    }

    public float CalculateNearPackDistRatio(float min, float max, float current)
    {
        if (current < min)
        {
            return 1;
        }
        else if (current > max)
        {
            return 0;
        }
        else
        {
            return 1 - (min / current);
        }
    }


}
