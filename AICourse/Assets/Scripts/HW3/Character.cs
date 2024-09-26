using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum CharacterState { Patrol, Chase,Run, Attack }
    public CharacterState _characterStateEnum;
    public float health;
    public Unit _pathFindUnit;
    protected StateSpeed _currentState;
    protected Vector3 _target;

    public virtual void Start()
    {
        _pathFindUnit = GetComponent<Unit>();
        SetPartolPoint();
    }

    public void SetPartolPoint()
    {
        int rnd = Random.Range(0, Grid.Instance.CheckPoints.Count);
        _target = Grid.Instance.CheckPoints[rnd];
        _pathFindUnit.SetDestanation(_target);
    }

    public virtual void SetState(StateSpeed stateVariables)
    {
        _currentState = stateVariables;
        _characterStateEnum = stateVariables.state;
        _pathFindUnit.speed = stateVariables.speed;

    }

    public virtual void Patrol()
    {

    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
