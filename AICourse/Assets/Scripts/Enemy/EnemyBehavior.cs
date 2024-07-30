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
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] Animator _animator;
    [SerializeField] EnemyState _enemyState;
    [SerializeField] float _attackDistance;

    [Header("Patrol")]
    Transform tempPoint;
    [SerializeField] List<Transform> _patrolPoints = new List<Transform>();

    [Header("State Speed")]
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
        _enemyState = EnemyState.Patrol;
        for (int i = 0; i < _sensors.Count; i++)
        {
            _sensors[i].OnSensorStart(_sensorPoint);
        }
    }

    private void Update()
    {
        if(_sensors.Count>0)
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
        switch (_enemyState)
        {
            case EnemyState.Patrol:
                _animator.SetFloat("Speed", patrol.speed);
                Patrol();
                break;
            case EnemyState.Chase:
                tempPoint = null;
                _animator.SetFloat("Speed", chase.speed);
                if (_target != null)
                {
                    _agent.SetDestination(_target.position);
                }
                break;
            case EnemyState.Search:
                tempPoint = null;
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
            if (Vector3.Distance(transform.position, tempPoint.position) <= .5f)
            {
                if (_currentTimer > 0)
                {
                    StateCD();
                    _animator.SetFloat("Speed", 0);
                }
                else
                    SetPartolPoint();
            }
        }
        else
        {
            SetPartolPoint();
        }
    }

    void SetPartolPoint()
    {
        int rnd= Random.Range(0, _patrolPoints.Count);
        tempPoint= _patrolPoints[rnd];
        _agent.SetDestination(tempPoint.position);
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
        _enemyState = stateVariables.state;
        _agent.speed = stateVariables.speed;
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
