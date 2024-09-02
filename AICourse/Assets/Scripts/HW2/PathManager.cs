using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public static PathManager Instance;

    Queue<PathRequest> pathRequests = new Queue<PathRequest>();
    PathFinding pathfinding;

    bool isProcessingPath;

    private void Awake()
    {
        Instance = this;
        pathfinding = GetComponent<PathFinding>();

    }



    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _pathStart, Vector3 _pathEnd, Action<Vector3[], bool> _callBack)
        {
            pathStart = _pathStart;
            pathEnd = _pathEnd;
            callback = _callBack;
        }
    }
}
