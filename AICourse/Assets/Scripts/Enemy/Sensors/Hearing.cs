using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hearing", menuName = "ScriptableObjects/Sensonrs/Hear")]
public class Hearing : SensorsSO
{
    [SerializeField] LayerMask _enemyLayer;

    public override void ExcuteMethod()
    {
        //set an array of all the object, with the specific layer, that entered the cast sphere
        Collider[] targetsInHearRadar = Physics.OverlapSphere(_sensorPoint.position, _range, _enemyLayer);

        if(targetsInHearRadar.Length>0)
        {
            _sensorDetection = true;
            _target = targetsInHearRadar[0].transform;
        }
        else
        {
            _sensorDetection = false;
            _target = null;
        }
    }
}
