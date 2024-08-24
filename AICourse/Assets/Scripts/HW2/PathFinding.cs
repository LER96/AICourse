using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    List<Node> _openSet = new List<Node>();
    HashSet<Node> _closeSet = new HashSet<Node>();
    Node _startNode;
    Node _targetNode;

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        _startNode = Grid.Instance.GetNodeWorldPoint(startPos);
        _targetNode= Grid.Instance.GetNodeWorldPoint(targetPos);
        _openSet.Add(_startNode);
    }

    private void Update()
    {
        if(_openSet.Count>0)
        {
            CheckNodeCost();
        }
    }

    void CheckNodeCost()
    {
        Node currentNode = _openSet[0];
        for (int i = 0; i < _openSet.Count; i++)
        {
            Node node = _openSet[i];
            if(node.FCost< currentNode.FCost || node.FCost== currentNode.FCost && node.hCost< currentNode.hCost)
            {
                currentNode = node;
            }
        }

        _openSet.Remove(currentNode);
        _closeSet.Add(currentNode);
        if (currentNode == _targetNode) return;
    }
}
