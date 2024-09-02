using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    bool pathSuccess;

    Heap<Node> _openSet;
    HashSet<Node> _closeSet = new HashSet<Node>();
    Stopwatch sw;


    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        sw = Stopwatch.StartNew();
        sw.Start();


        Vector3[] waypoints = new Vector3[0];
        pathSuccess = false;

        //set Start Node, End Node
        Node _startNode = Grid.Instance.GetNodeWorldPoint(startPos);
        Node _targetNode = Grid.Instance.GetNodeWorldPoint(targetPos);

        if (_startNode.walkable && _targetNode.walkable)
        {
            _openSet = new Heap<Node>(Grid.Instance.MaxSize);
            _closeSet.Clear();

            _openSet.Add(_startNode);

            while (_openSet.Count > 0)
            {
                Node currentNode = _openSet.RemoveFirst();
                _closeSet.Add(currentNode);

                if (currentNode == _targetNode)
                {
                    sw.Stop();
                    pathSuccess = true;
                    RetracePath(_startNode, _targetNode);
                    break;
                }

                List<Node> neighbors = Grid.Instance.GetNeighbors(currentNode);

                foreach (Node neighbor in neighbors)
                {
                    if (!neighbor.walkable || _closeSet.Contains(neighbor))
                        continue;

                    int neighborCost = currentNode.gCost + GetDistance(currentNode, neighbor);

                    if (neighborCost < neighbor.gCost || !_openSet.Contains(neighbor))
                    {
                        neighbor.gCost = neighborCost;
                        neighbor.hCost = GetDistance(neighbor, _targetNode);
                        neighbor.parent = currentNode;

                        if (!_openSet.Contains(neighbor))
                        {
                            _openSet.Add(neighbor);
                        }
                        else
                        {
                            _openSet.UpdateItem(neighbor);
                        }
                    }
                }
            }
        }

        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(_startNode, _targetNode);
        }
        PathManager.Instance.FinishProcessPath(waypoints, pathSuccess);
    }

    Vector3[] RetracePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node _current = end;
        while(_current!= start)
        {
            path.Add(_current);
            _current = _current.parent;
        }
        Vector3[] waypoints= SimplfyPath(path);
        Array.Reverse(waypoints);
        return waypoints;

    }

    Vector3[] SimplfyPath(List<Node>path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 dirrection = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 dirrectionNew = new Vector2(path[i-1].XPos- path[i].XPos, path[i - 1].YPos - path[i].YPos);
            if(dirrectionNew!= dirrection)
            {
                waypoints.Add(path[i].worldPosition);
            }
            dirrection = dirrectionNew;
        }
        return waypoints.ToArray();
    }

    int GetDistance(Node a, Node b)
    {
        int disX = Mathf.Abs(a.XPos - b.XPos);
        int disY = Mathf.Abs(a.YPos - b.YPos);

        if (disX > disY)
            return 14 * disY + 10*(disX - disY);
        return 14 * disX + 10 * (disY - disX);
    }
}
