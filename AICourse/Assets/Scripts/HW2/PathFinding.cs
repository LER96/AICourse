using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    private Transform seeker, target;

    Heap<Node> _openSet;
    HashSet<Node> _closeSet = new HashSet<Node>();
    Node _startNode;
    Node _targetNode;
    Stopwatch sw;

    private void Start()
    {
        seeker = this.transform;
        target= Player.Instance.transform;

    }

    private void Update()
    {
            FindPath(seeker.position, target.position);
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        sw = Stopwatch.StartNew();
        sw.Start();
        _openSet = new Heap<Node>(Grid.Instance.MaxSize);
        _closeSet.Clear();

        _startNode = Grid.Instance.GetNodeWorldPoint(startPos);
        _targetNode= Grid.Instance.GetNodeWorldPoint(targetPos);
        _openSet.Add(_startNode);

        while (_openSet.Count > 0)
        {
            CheckNodeCost();
        }
    }

    void CheckNodeCost()
    {
        Node currentNode = _openSet.RemoveFirst();
        _closeSet.Add(currentNode);

        if (currentNode == _targetNode)
        {
            sw.Stop();
            RetracePath(_startNode, _targetNode);
            return;
        }

        List<Node> neighbors = Grid.Instance.GetNeighbors(currentNode);

        foreach (Node neighbor in neighbors)
        {
            if (!neighbor.walkable || _closeSet.Contains(neighbor))
                continue;

            int neighborCost = currentNode.gCost + GetDistance(currentNode, neighbor);

            if(neighborCost< neighbor.gCost || !_openSet.Contains(neighbor))
            {
                neighbor.gCost = neighborCost;
                neighbor.hCost = GetDistance(neighbor, _targetNode);
                neighbor.parent = currentNode;

                if(!_openSet.Contains(neighbor))
                {
                    _openSet.Add(neighbor);
                }
            }
        }
    }

    void RetracePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node _current = end;
        while(_current!= start)
        {
            path.Add(_current);
            _current = _current.parent;
        }
        path.Reverse();

        Grid.Instance.path = path;
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
