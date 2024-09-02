using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node: IHeapItem<Node>
{
    public Node parent;

    public bool walkable;
    public Vector3 worldPosition;
    public int XPos;
    public int YPos;

    public int gCost;
    public int hCost;

    int heapIndex;

    public Node( bool walk, Vector3 worldPos, int x, int y)
    {
        walkable = walk;
        worldPosition = worldPos;
        XPos = x;
        YPos = y;
    }

    public int FCost { get { return gCost + hCost; } }

    public int HeapIndex 
    {
        get { return heapIndex; }
        set { heapIndex = value; }
    }

    public int CompareTo(Node other)
    {
        int compar= FCost.CompareTo(other.FCost);
        if(compar == 0 )
        {
            compar= hCost.CompareTo(other.hCost);

        }
        return -compar;
    }
}
