using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Node parent;

    public bool walkable;
    public Vector3 worldPosition;
    public int XPos;
    public int YPos;

    public int gCost;
    public int hCost;

    public Node( bool walk, Vector3 worldPos, int x, int y)
    {
        walkable = walk;
        worldPosition = worldPos;
        XPos = x;
        YPos = y;
    }

    public int FCost { get { return gCost + hCost; } }
}
