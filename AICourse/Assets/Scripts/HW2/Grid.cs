using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask unWalkable;
    public Vector2 gridSize;
    public float nodeRadius;
    Node[,] grid;

    float _nodeDimantions;
    int _grisdSizeX, _grisdSizeY;

    private void Start()
    {
        _nodeDimantions = nodeRadius * 2;
        _grisdSizeX = (int) (gridSize.x / _nodeDimantions);
        _grisdSizeY = (int)(gridSize.y / _nodeDimantions);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[_grisdSizeX, _grisdSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridSize.x / 2 - Vector3.forward * gridSize.y / 2;

        for (int x = 0; x < _grisdSizeX; x++)
        {
            for (int y = 0; y < _grisdSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + 
                    Vector3.right * (x * _nodeDimantions + nodeRadius) + 
                    Vector3.forward * (y * _nodeDimantions + nodeRadius);

                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unWalkable));
                grid[x, y] = new Node(walkable, worldPoint);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, new Vector3(gridSize.x, 1, gridSize.y));

        if(grid!=null)
        {
            foreach(Node node in grid)
            {
                Gizmos.color = (node.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (_nodeDimantions - 0.1f));
            }
        }
    }
}
