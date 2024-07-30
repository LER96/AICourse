using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SensorsSO))]
public class SightViewEditor : Editor
{
    private void OnSceneGUI()
    {
        SensorsSO fow = (SensorsSO)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.Sensor.position, Vector3.up, Vector3.right, 360, fow.Range);

        //Vector3 pointA = fow.DirFromAngle(-fow.viewAngle, false);
        //Vector3 pointB = fow.DirFromAngle(fow.viewAngle, false);

        //Handles.DrawLine(fow.transform.position, fow.transform.position + pointA * fow.viewRadius);
        //Handles.DrawLine(fow.transform.position, fow.transform.position + pointB * fow.viewRadius);

        //Handles.color = Color.red;
        //foreach (Transform visibleTarget in fow.visibleTargets)
        //{
        //    Handles.DrawLine(fow.transform.position, visibleTarget.position);
        //}
    }
}
