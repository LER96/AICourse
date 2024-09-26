using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hearing", menuName = "ScriptableObjects/Sensonrs/Hear")]
public class Hearing : SensorsSO
{

    public override void ExcuteMethod()
    {
        Collider[] targetsInFieldView = Physics.OverlapSphere(_sensorPoint.position, _range, _enemyLayer);
        if (targetsInFieldView.Length > 0)
        {
            if (targetsInFieldView[0].CompareTag("Player"))
            {
                player = targetsInFieldView[0].GetComponent<GeneticAgent>();
                if (player != null)
                {
                    _sensorDetection = true;
                    player.detected = true;
                    _target = targetsInFieldView[0].transform;
                }
            }
        }
        else
        {
            _sensorDetection = false;
            _target = null;
            player = null;
        }
    }


}
