using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int GridX; // ������� ���� �� �
    public int GridY; // ������� ���� �� Y

    public bool IsWall; // �������� �� ���� ������
    public Vector3 Position; // ������� ���� � ������� ������������

    public Node Parent; // ������ �����

    public int GCost; // ���������� �� ���������� ����
    public int HCost; // ���������� �� ��������� ����
    public int FCost => GCost + HCost; // ����� ��������� �� ����

    public Node(bool isWall, Vector3 position, int gridX, int gridY)
    {
        IsWall = isWall;
        Position = position;
        GridX = gridX;
        GridY = gridY;
    }
    
}
