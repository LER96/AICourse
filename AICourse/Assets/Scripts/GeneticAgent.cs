using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAgent : Character
{
    [Header("Weights")]
    [Range(0,1)]
    public float healthPriority;
    [Range(0, 1)]
    public float distancePriority;
    [Range(0, 1)]
    public float movementPriority;
    public float fitness;
    public bool detected;

    [Header("States")]
    [SerializeField] StateSpeed patrol;
    [SerializeField] StateSpeed escape;

    [Header("Max Variables")]
    [SerializeField] float maxDistance;
    [SerializeField] float maxMovmement;
    float _maxHp;
    Transform tempLocation;
    public override void Start()
    {
        _maxHp = health;
        InitWeights();
        OnStart();
    }
    public void OnStart()
    {
        health = _maxHp;
        SetPartolPoint();
        SetState(patrol);
    }
    void InitWeights()
    {
        healthPriority = Random.Range(0, 2);
        distancePriority = Random.Range(0, 2);
        movementPriority = Random.Range(0, 2);
    }
    private void Update()
    {
        CheckDetected();
        ManageState();
    }

    void ManageState()
    {
        switch(_characterStateEnum)
        {
            case CharacterState.Patrol:
                Patrol();
                break;
            case CharacterState.Run:

                break;
            case CharacterState.Attack:
                break;
        }
    }

    void CheckCloseHealthPack()
    {
        float nearbyPack = 6000;
        foreach (Transform pack in EnvironmentManager.Instance.HealthPacks)
        {
            tempLocation = pack;
            float dist = Vector3.Distance(transform.position, pack.transform.position);
            if (dist < nearbyPack)
            {
                tempLocation = pack;
                nearbyPack = dist;
            }
        }
        _target = tempLocation.transform.position;
        _pathFindUnit.SetDestanation(_target);
    }

    void CheckDetected()
    {
        if(detected)
        {

        }

        if (detected)
        {
            SetState(escape);
        }
        else
        {
            SetState(patrol);
        }

    }

    public override void Patrol()
    {
        float dist = Vector3.Distance(transform.position, _target);
        if (dist <= 1f)
        {
            SetPartolPoint();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            EnemyBehavior enemy = other.GetComponent<EnemyBehavior>();
            if(enemy!=null)
            {
                TakeDamage(enemy.Damage);
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

}
