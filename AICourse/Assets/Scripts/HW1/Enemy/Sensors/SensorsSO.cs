using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SensorsSO : ScriptableObject
{

    [SerializeField] protected float _range;
    [SerializeField] protected LayerMask _enemyLayer;
    protected bool _sensorDetection;
    protected Transform _sensorPoint;
    protected Transform _target;
    protected GeneticAgent player;
    protected Collider[] targetsInFieldView;

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
        //set an array of all the object, with the specific layer, that entered the cast sphere
        Collider[] targetsInFieldView = Physics.OverlapSphere(_sensorPoint.position, _range, _enemyLayer);
    }




}
