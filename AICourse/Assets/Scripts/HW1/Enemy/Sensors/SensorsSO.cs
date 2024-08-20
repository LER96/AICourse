using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SensorsSO : ScriptableObject
{

    [SerializeField] protected float _range;
    protected bool _sensorDetection;
    protected Transform _sensorPoint;
    protected Transform _target;

    public Transform Target => _target;
    public Transform Sensor=> _sensorPoint;
    public float Range => _range;
    public bool Detected => _sensorDetection;

    public virtual void OnSensorStart(Transform sensorPoint)
    {
        _sensorPoint = sensorPoint;
    }

    public virtual void ExcuteMethod()
    {

    }


}
