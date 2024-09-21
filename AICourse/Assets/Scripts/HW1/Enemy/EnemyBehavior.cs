using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    public enum EnemyState { Patrol, Chase, Search, Attack }

    [System.Serializable]
    public class StateSpeed
    {
        public EnemyState state;
        public float speed;
        public float cdTime;
    }

    [Header("Basic Variables")]
    [SerializeField] Transform _sensorPoint;
    [SerializeField] Animator _animator;
    [SerializeField] Unit _pathFindUnit;
    [SerializeField] EnemyState _enemyStateEnum;
    [SerializeField] float _attackDistance;
    [SerializeField] float _chaseDistance;


    [Header("State Speed")]
    StateSpeed _currentState;
    [SerializeField] StateSpeed patrol;
    [SerializeField] StateSpeed chase;
    [SerializeField] StateSpeed search;
    [SerializeField] StateSpeed attack;

    [Header("Sensors")]
    [SerializeField] List<SensorsSO> _sensors = new List<SensorsSO>();

    SensorsSO _currentSensor;
    [SerializeField] Transform _target;
    Transform _lastTargetPos;
    float _currentTimer;
    float _copyTime;

    private void Start()
    {
        _pathFindUnit = GetComponent<Unit>();
        SetPartolPoint();
        SetState(patrol);

        for (int i = 0; i < _sensors.Count; i++)
        {
            _sensors[i].OnSensorStart(_sensorPoint);
        }
    }

    private void Update()
    {
        if (_sensors.Count > 0)
        {
            UpdateSensors();
        }
        ManageBehavior();
    }

    void UpdateSensors()
    {
        for (int i = 0; i < _sensors.Count; i++)
        {
            _sensors[i].ExcuteMethod();
            if (_sensors[i].Detected)
            {
                _target = _sensors[i].Target;
                SetState(chase);
                break;
            }
        }
    }

    void CheckInDistance()
    {
        float dist = Vector3.Distance(transform.position, _target.position);
        if (dist <= _attackDistance)
        {
            SetState(attack);
        }
        else if(dist <=_chaseDistance)
        {
            SetState(chase);
        }
    }

    void ManageBehavior()
    {
        if (_target != null)
        {
            _pathFindUnit.SetDestanation(_target);
        }
        switch (_enemyStateEnum)
        {
            case EnemyState.Patrol:
                _animator.SetFloat("Speed", patrol.speed);
                Patrol();
                break;
            case EnemyState.Chase:
                _animator.SetFloat("Speed", chase.speed);
                CheckInDistance();
                break;
            case EnemyState.Attack:
                _animator.SetFloat("Speed", attack.speed);
                Attack();
                break;
        }
    }

    #region Patrol
    void Patrol()
    {
        if (Vector3.Distance(transform.position, _target.position) <= 3f)
        {
            if (_currentTimer > 0)
            {
                StateCD();
                //_animator.SetFloat("Speed", 0);
            }
        }
    }

    void SetPartolPoint()
    {
        int rnd= Random.Range(0, Grid.Instance.CheckPoints.Count);
        _target= Grid.Instance.CheckPoints[rnd];
        _pathFindUnit.SetDestanation(_target);
        //_agent.SetDestination(tempPoint.position);
    }
    #endregion

    void Search()
    {
        if (_currentTimer > 0)
        {
            Patrol();
            StateCD();
            _animator.SetFloat("Speed", 0);
        }
        else
            SetState(patrol);
    }

    void Attack()
    {
        if (_currentTimer > 0)
        {
            StateCD();
        }
        else
        {
            Debug.Log("Attack");
            _currentTimer = _copyTime;
        }
    }

    void SetState(StateSpeed stateVariables)
    {
        _currentState = stateVariables;
        _enemyStateEnum = stateVariables.state;
        _pathFindUnit.speed = stateVariables.speed;
        _currentTimer = stateVariables.cdTime;
        _copyTime = _currentTimer;
    }

    void StateCD()
    {
        _currentTimer -= Time.deltaTime;
        if (_currentTimer < 0)
        {
            _currentTimer = _copyTime;
            SetPartolPoint();
        }
    }
}
