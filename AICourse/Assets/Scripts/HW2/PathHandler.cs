using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathHandler : MonoBehaviour
{
    Queue<PathRequest> pathRequests = new Queue<PathRequest>();
    PathRequest currentRequest;
    PathFinding pathfinding;

    bool isProcessingPath;

    private void Awake()
    {
        pathfinding = GetComponent<PathFinding>();
    }

    public void RequestPath(Vector3 start, Vector3 end, Action<Vector3[], bool> callbacks)
    {
        PathRequest newRequest = new PathRequest(start, end, callbacks);
        pathRequests.Enqueue(newRequest);
        TryProcessNext();
    }

    void TryProcessNext()
    {
        if (!isProcessingPath && pathRequests.Count > 0)
        {
            currentRequest = pathRequests.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentRequest.pathStart, currentRequest.pathEnd);
        }
    }

    public void FinishProcessPath(Vector3[] path, bool success)
    {
        currentRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
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
