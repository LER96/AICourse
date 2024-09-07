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

    [Header("Patrol")]
    Transform tempPoint;
    [SerializeField] List<Transform> _patrolPoints = new List<Transform>();

    [Header("State Speed")]
    StateSpeed _currentState;
    [SerializeField] StateSpeed patrol;
    [SerializeField] StateSpeed chase;
    [SerializeField] StateSpeed search;
    [SerializeField] StateSpeed attack;

    [Header("Sensors")]
    [SerializeField] List<SensorsSO> _sensors = new List<SensorsSO>();

    SensorsSO _currentSensor;
    Transform _target;
    Transform _lastTargetPos;
    float _currentTimer;
    float _copyTime;

    private void Start()
    {
        _pathFindUnit = GetComponent<Unit>();
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
            ManageSensors();
            if (_target != null)
            {
                CheckInDistance();
            }
        }
        ManageBehavior();
    }

    void UpdateSensors()
    {
        for (int i = 0; i < _sensors.Count; i++)
        {
            _sensors[i].ExcuteMethod();
        }
    }

    void ManageSensors()
    {
        for (int i = 0; i < _sensors.Count; i++)
        {
            if(_sensors[i].Detected && _target==null)
            {
                SetState(chase);
                _currentSensor = _sensors[i];
                _target = _sensors[i].Target;
                break;
            }
        }
        if (_currentSensor != null)
        {
            LostTarget();
        }
    }

    void LostTarget()
    {
        if (_currentSensor.Target == null)
        {
            _target = null;
            SetState(search);
        }
    }

    void CheckInDistance()
    {
        if(Vector3.Distance(transform.position, _target.position)<= _attackDistance)
        {
            SetState(attack);
        }
        else
        {
            SetState(chase);
        }
    }


    void ManageBehavior()
    {
        switch (_enemyStateEnum)
        {
            case EnemyState.Patrol:
                _animator.SetFloat("Speed", patrol.speed);
                Patrol();
                break;
            case EnemyState.Chase:
                _animator.SetFloat("Speed", chase.speed);
                if (_target != null)
                {
                    _pathFindUnit.SetDestanation(Player.Instance.transform);
                }
                break;
            case EnemyState.Search:
                _animator.SetFloat("Speed", search.speed);
                Search();
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
        if(tempPoint!=null)
        {
            if (Vector3.Distance(transform.position, tempPoint.position) <= 3f)
            {
                if (_currentTimer > 0)
                {
                    StateCD();
                    _animator.SetFloat("Speed", 0);
                }
                else
                {
                    tempPoint = null;
                }
            }
        }
        else
        {
            _currentTimer = _copyTime;
            SetPartolPoint();
        }
    }

    void SetPartolPoint()
    {
        int rnd= Random.Range(0, _patrolPoints.Count);
        tempPoint= _patrolPoints[rnd];
        _pathFindUnit.SetDestanation(tempPoint);
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
        if (_currentTimer < 0 )
            _currentTimer = 0;
    }
}
