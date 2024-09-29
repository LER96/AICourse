using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : Character
{

    [Header("Basic Variables")]
    [SerializeField] Transform _sensorPoint;
    [SerializeField] Animator _animator;
    [SerializeField] float _attackDistance;
    [SerializeField] float damage;

    [Header("State Speed")]
    [SerializeField] StateSpeed patrol;
    [SerializeField] StateSpeed chase;
    [SerializeField] StateSpeed attack;

    [Header("Sensors")]
    [SerializeField] List<SensorsSO> _sensors = new List<SensorsSO>();

    [SerializeField] SensorsSO _currentSensor;
    float _currentTimer;
    float _copyTime;
    Vector3 newTarget;

    public float Damage => damage;

    public override void Start()
    {
        base.Start();
        SetState(patrol);

        //Set Seonsor transform
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
            // if sensor detected the player
            if (_sensors[i].Detected)
            {
                _currentSensor = _sensors[i];
                _target = _sensors[i].Target.position;
                newTarget = new Vector3(_target.x, 0, _target.z);
                _pathFindUnit.SetDestanation(newTarget);
                SetState(chase);
                break;
            }
        }
    }

    void CheckInDistance()
    {
        if(_currentSensor.Detected==false)
        {
            SetState(patrol);
        }
    }

    void ManageBehavior()
    {
        switch (_characterStateEnum)
        {
            case CharacterState.Patrol:
                _animator.SetFloat("Speed", patrol.speed);
                Patrol();
                break;
            case CharacterState.Chase:
                _animator.SetFloat("Speed", chase.speed);
                CheckInDistance();
                break;
        }
    }

    public override void Patrol()
    {
        if (_pathFindUnit.reachedTarget)
        {
            if (_currentTimer > 0)
            {
                StateCD();
                _animator.SetFloat("Speed", 0);
            }
        }
        
    }


    public override void SetState(StateSpeed stateVariables)
    {
        _currentState = stateVariables;
        _characterStateEnum = stateVariables.state;
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

    private void OnDrawGizmos()
    {
        foreach (SensorsSO sensor in _sensors)
        {
            Gizmos.DrawLine(sensor.Sensor.position, newTarget);
            Gizmos.DrawWireSphere(transform.position, sensor.Range);
        }
    }
}
