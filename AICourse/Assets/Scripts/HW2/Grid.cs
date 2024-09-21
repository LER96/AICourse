using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public static Grid Instance;

    public bool displayGizmos;
    public LayerMask unWalkable;
    public Vector2 gridSize;
    float nodeRadius;

    [Header("Patrol")]
    Transform tempPoint;
    [SerializeField] List<Transform> _patrolPoints = new List<Transform>();

    Node[,] _grid;
    float _nodeDimantions;
    int _grisdSizeX, _grisdSizeY;

    public List<Transform> CheckPoints => _patrolPoints;
    public int MaxSize { get {  return _grisdSizeX * _grisdSizeY; } }

    private void Awake()
    {
        Instance = this;
        nodeRadius = gridSize.x / 100;
        _nodeDimantions = nodeRadius * 2;
        _grisdSizeX = (int)(gridSize.x / _nodeDimantions);
        _grisdSizeY = (int)(gridSize.y / _nodeDimantions);
        CreateGrid();
    }


    void CreateGrid()
    {
        _grid = new Node[_grisdSizeX, _grisdSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridSize.x / 2 - Vector3.forward * gridSize.y / 2;

        for (int x = 0; x < _grisdSizeX; x++)
        {
            for (int y = 0; y < _grisdSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + 
                    Vector3.right * (x * _nodeDimantions + nodeRadius) + 
                    Vector3.forward * (y * _nodeDimantions + nodeRadius);

                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unWalkable));
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

                if(checkX>=0 && checkX< _grisdSizeX && checkY >= 0 && checkY < gridSize.y)
                {
                    neighbors.Add(_grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    public Node GetNodeWorldPoint(Vector3 worldPos)
    {
        float precentX = (worldPos.x + gridSize.x / 2) / gridSize.x;
        float precentY = (worldPos.z + gridSize.y / 2) / gridSize.y;
        precentX = Mathf.Clamp01(precentX);
        precentY = Mathf.Clamp01(precentY);

        int x = Mathf.RoundToInt((gridSize.x - 1) * precentX);
        int y = Mathf.RoundToInt((gridSize.y - 1) * precentY);

        return _grid[x, y];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, new Vector3(gridSize.x, 0.5f, gridSize.y));
        if (_grid != null && displayGizmos)
        {
            foreach (Node node in _grid)
            {
                Gizmos.color = (node.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (_nodeDimantions - 0.1f));
            }
        }
    }
}
