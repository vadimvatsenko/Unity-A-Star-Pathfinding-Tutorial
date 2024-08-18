using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int GridX; // позиция узла по Х
    public int GridY; // позиция узла по Y

    public bool IsWall; // является ли узел стеной
    public Vector3 Position; // позиция узла в мировом пространстве

    public Node Parent; // соседи узлов

    public int GCost; // расстояние от начального узла
    public int HCost; // расстояние до конечного узла
    public int FCost => GCost + HCost; // общая стоимость до цели

    public Node(bool isWall, Vector3 position, int gridX, int gridY)
    {
        IsWall = isWall;
        Position = position;
        GridX = gridX;
        GridY = gridY;
    }
    
}
