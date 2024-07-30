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
    float _timer;

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
            _lastTargetPos = _target;
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
        switch(_enemyState)
        {
            case EnemyState.Patrol:
                _animator.SetFloat("Speed", patrol.speed);
                break;
            case EnemyState.Chase:
                _animator.SetFloat("Speed", chase.speed);
                if (_target != null)
                {
                    _agent.SetDestination(_target.position);
                }
                break;
            case EnemyState.Search:
                _animator.SetFloat("Speed", search.speed);
                if (_lastTargetPos != null)
                {
                    _agent.SetDestination(_lastTargetPos.position);
                }
                break;
            case EnemyState.Attack:

                Debug.Log("Attack");
                break;
        }
    }

    void SetState(StateSpeed stateVariables)
    {
        _enemyState = stateVariables.state;
        _agent.speed = stateVariables.speed;
        _timer = stateVariables.cdTime;
    }
}
