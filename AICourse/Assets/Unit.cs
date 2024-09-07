using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float speed;
    private Transform target;
    Vector3[] path;
    int targetIndex;

    public Transform Target => target;

    public void SetDestanation(Transform _target)
    {
        target = _target;
        PathManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccesful)
    {
        if(pathSuccesful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        targetIndex = 0;
        Vector3 currentWaypoint = path[0];
        while(true)
        {
            if(transform.position== currentWaypoint)
            {
                targetIndex++;
                if(targetIndex >= path.Length)
                {
                    //target = null;
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            transform.LookAt(currentWaypoint);
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed*Time.deltaTime);
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if(path!=null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if(i==targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i-1], path[i]);
                }
            }
        }
    }
}
