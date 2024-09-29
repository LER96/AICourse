using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sight", menuName = "ScriptableObjects/Sensonrs/Sight")]
public class Sight : SensorsSO
{
    [Range(0, 360)]
    [SerializeField] float viewAngle;
    [SerializeField] LayerMask _obsMask;
    List<Transform> _targetOnSight = new List<Transform>();

    public override void ExcuteMethod()
    {
        _targetOnSight.Clear();
        //set an array of all the object, with the specific layer, that entered the cast sphere
        Collider[] targetsInFieldView = Physics.OverlapSphere(_sensorPoint.position, _range, _enemyLayer);
        for (int i = 0; i < targetsInFieldView.Length; i++)
        {
            Transform target = targetsInFieldView[i].transform;
            Vector3 dirToTarget = (target.position - _sensorPoint.position).normalized;

            //if the position is on the middle of the camera view// that means the player is looking right at it
            if (Vector3.Angle(_sensorPoint.forward, dirToTarget) < viewAngle)
            {
                float distTarget = Vector3.Distance(_sensorPoint.position, dirToTarget);

                bool hit = Physics.Raycast(_sensorPoint.position, dirToTarget, _range, _obsMask);
                //cast a ray that make sure that the target is not hiding behind anything
                if (hit==false)
                {
                    _targetOnSight.Add(target);
                }
            }
        }

        if (_targetOnSight.Count > 0)
        {
            if (targetsInFieldView[0].CompareTag("Player"))
            {
                player = targetsInFieldView[0].GetComponent<GeneticAgent>();
                if (player != null)
                {
                    _sensorDetection = true;
                    _target = targetsInFieldView[0].transform;
                }
            }
        }
        else
        {
            _sensorDetection = false;
            _target = null;
        }

        if(player!=null)
        {
            player.detected = _sensorDetection;
        }
    }
}
