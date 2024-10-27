using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentGrid : MonoBehaviour
{
    public bool displayGizmos;
    public LayerMask unWalkable;
    public int gridDivider;
    [Header("Environment Variables")]
    public Transform _floor;
    public Transform _parent;
    float nodeRadius;
    Vector2 gridSize;

    [Header("Patrol")]
    Transform tempPoint;
    List<Vector3> _patrolPoints = new List<Vector3>();

    Node[,] _grid;
    float _nodeDimantions;
    int _grisdSizeX, _grisdSizeY;

    public List<Vector3> CheckPoints => _patrolPoints;
    public int MaxSize { get {  return _grisdSizeX * _grisdSizeY; } }

    private void Awake()
    {
        gridSize = new Vector2(_floor.localScale.x, _floor.localScale.z);
        nodeRadius = gridSize.x / gridDivider;
        _nodeDimantions = nodeRadius * 2;
        _grisdSizeX = (int)(gridSize.x / _nodeDimantions);
        _grisdSizeY = (int)(gridSize.y / _nodeDimantions);
        CreateGrid();
    }


    void CreateGrid()
    {
        _grid = new Node[_grisdSizeX, _grisdSizeY];
        Vector3 worldBottomLeft = _parent.position - Vector3.right * gridSize.x / 2 - Vector3.forward * gridSize.y / 2;

        for (int x = 0; x < _grisdSizeX; x++)
        {
            for (int y = 0; y < _grisdSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + 
                    Vector3.right * (x * _nodeDimantions + nodeRadius) + 
                    Vector3.forward * (y * _nodeDimantions + nodeRadius);

                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unWalkable));
                if(walkable==false)
                {
                    Debug.Log(worldPoint);
                }
                _patrolPoints.Add(worldPoint);
                _grid[x, y] = new Node(walkable, worldPoint,x,y);
            }
        }
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = node.XPos + x;
                int checkY = node.YPos + y;

                if(checkX>=0 && checkX< _grisdSizeX && checkY >= 0 && checkY < _grisdSizeY)
                {
                    neighbors.Add(_grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    public Node GetNodeWorldPoint(Vector3 worldPos)
    {
        float precentX = (worldPos.x + _grisdSizeX / 2) / _grisdSizeX;
        float precentY = (worldPos.z + _grisdSizeY / 2) / _grisdSizeY;
        precentX = Mathf.Clamp01(precentX);
        precentY = Mathf.Clamp01(precentY);

        int x = Mathf.RoundToInt((_grisdSizeX - 1) * precentX);
        int y = Mathf.RoundToInt((_grisdSizeY - 1) * precentY);

        return _grid[x, y];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(_parent.position, new Vector3(gridSize.x, 0.5f, gridSize.y));
    }
}
