using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float speed;
    public bool reachedTarget;
    public PathHandler pathManager;
    private Transform target;
    Vector3 targetLocation;
    Vector3[] path;
    int targetIndex;

    public Transform Target => target;

    public void SetDestanation(Transform _target)
    {
        target = _target;
        targetLocation = target.position;
        SetPath();
    }

    public void SetDestanation(Vector3 location)
    {
        targetLocation = location;
        SetPath();
    }

    void SetPath()
    {
        pathManager.RequestPath(transform.position, targetLocation, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccesful)
    {
        if(pathSuccesful && newPath.Length>0)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
        else
        {
            SetPath();
        }
    }

    IEnumerator FollowPath()
    {
        targetIndex = 0;
        reachedTarget = false;
        Vector3 currentWaypoint = path[0];
        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    //target = null;
                    reachedTarget = true;
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            transform.LookAt(currentWaypoint);
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
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
