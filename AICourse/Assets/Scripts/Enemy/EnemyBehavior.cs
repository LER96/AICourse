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
        public float _cdTime;
    }

    [Header("Basic Variables")]
    [SerializeField] Transform _sensorPoint;
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] Animator _animator;
    [SerializeField] EnemyState _enemyState;
    [SerializeField] float _attackDistance;
    [SerializeField] float _speed;

    [Header("State Speed")]
    [SerializeField] StateSpeed patrol;
    [SerializeField] StateSpeed chase;
    [SerializeField] StateSpeed search;
    [SerializeField] StateSpeed attack;

    [Header("Sensors")]
    [SerializeField] List<SensorsSO> _sensors = new List<SensorsSO>();

    Transform _target;
    Vector3 _lastPos;

    private void Start()
    {
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
                _target = _sensors[i].Targets[0];
                break;
            }
        }

    }

    void CheckInDistance()
    {
        if(Vector3.Distance(transform.position, _target.position)<= _attackDistance)
        {
            SetState(attack);
        }
    }


    void ManageBehavior()
    {
        switch(_enemyState)
        {
            case EnemyState.Patrol:
                
                break;
            case EnemyState.Chase:
                if (_target != null)
                {
                    _agent.SetDestination(_target.position);
                }
                break;
            case EnemyState.Search:

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
        _speed= stateVariables.speed;
    }
}
